using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class StateMachine : GameScript {
	protected class SMState {
		public string stateName;
		
		public Hashtable legalTransitions;
		
		public SMState( string name ) {
			this.stateName = name;
			this.legalTransitions = new Hashtable();
		}
	}
	
	protected class SMStateTransition {
		public SMState targetState;
		
		public float delay;
		public object[] args;
		
		public SMStateTransition( SMState targetState, float delay, object[] args ) {
			this.targetState = targetState;
			this.delay = delay;
			this.args = args;
		}
	}
	
	Hashtable legalStates;

	protected bool debugStates = false;
	protected SMState currentState;
	protected SMState newState;
	protected SMState oldState;
	
	protected MethodInfo updateMethodInfo;
	
	Timer transitionTimer;
	SMStateTransition timedTransition;
	Queue<SMStateTransition> transitionQueue;
	
	protected bool  isTransitioning;
	
	protected new void Start() {
		base.Start();
		
		legalStates = new Hashtable();		
		updateMethodInfo = null;
		transitionTimer = null;
		isTransitioning = false;
		
		transitionQueue = new Queue<SMStateTransition>();
		currentState = this.AddState( "Start", null );
	}
	
	protected void Update() {
		if( updateMethodInfo != null ) {
			updateMethodInfo.Invoke( this, null );
		}
		
		if( !isTransitioning && transitionQueue.Count > 0 ) {
			DequeueTransitionsUntilEmpty();
		}		
	}
	
	protected bool CanTransitionTo( string newstate ) {
		// can transition to any existing state from the null state		
		if( currentState != null && currentState.stateName == "Start"  ) {
			if( legalStates.ContainsKey( newstate ) ) 
				return true;
			else
				return false;
		}
		
		if( isTransitioning )
			return false;

		return legalStates.ContainsKey( newstate ) && ((SMState)currentState).legalTransitions.ContainsKey(newstate);
	}
	
	protected SMState AddState( string name, params string[] legalTransitions ) {
		SMState newstate = new SMState( name );

		if( legalStates.ContainsKey( name ) ) {
			Debug.LogWarning( "THStateMachine:: attempted to add duplicate state " + name + " => ignored");
			return null;
		}

		if( legalTransitions != null ) {
			for( int i = 0; i < legalTransitions.Length; i++ ) {
				if( newstate.legalTransitions.ContainsKey( legalTransitions[i]) ) {
					Debug.LogWarning( "THStateMachine:: attempted to add duplicate transition " + legalTransitions[i] + " for state " + name );
					continue;
				}

				newstate.legalTransitions.Add( legalTransitions[i], true );
			}			
		}

		legalStates.Add( name, newstate );

		return newstate;
	}
	
	protected MethodInfo TransitionMethodForState( string transition, string name ) {
		Type t = this.GetType();
		MethodInfo mi;
		
		mi = t.GetMethod( transition + name, BindingFlags.NonPublic | BindingFlags.Instance );
		if( mi == null ) {
			// could not find a custom method, fall back to the default one
			
			mi = t.GetMethod( "Default"+transition, BindingFlags.NonPublic | BindingFlags.Instance );
		}
		
		return mi;
	}
	
	private void DequeueTransitionsUntilEmpty() {
		while( transitionQueue.Count > 0 && !isTransitioning ) {
			SMStateTransition trans = transitionQueue.Dequeue();
			this.RequestState( trans.targetState.stateName, trans.delay, trans.args );
		}
	}
	
	private void TransitionToState( string name, object[] args ) {
		MethodInfo enterMethod, exitMethod;
		
		isTransitioning = true;
		
		this.newState = (SMState)legalStates[name];
		this.oldState = this.currentState;
		this.currentState = null;  // not yet in any state...
		
		exitMethod = TransitionMethodForState( "Exit", this.oldState.stateName );
		if( exitMethod != null ) {
			ParameterInfo[] pinfo = exitMethod.GetParameters();
			
			if( pinfo.Length == 0 )
				exitMethod.Invoke( this, null );
			else
				exitMethod.Invoke( this, args );
		}
		
		enterMethod = TransitionMethodForState( "Enter", this.newState.stateName );
		if( enterMethod != null )  {
			ParameterInfo[] pinfo = enterMethod.GetParameters();
			
			if( pinfo.Length == 0 )
				enterMethod.Invoke( this, null );
			else
				enterMethod.Invoke( this, args );
		}
		
		this.currentState = this.newState;
		this.newState = null;
		// perhaps should clear oldstate too?
		
		// figure out the new update method...
		Type t = this.GetType();
		updateMethodInfo = t.GetMethod( "Update"+this.currentState.stateName, BindingFlags.NonPublic | BindingFlags.Instance );
		if( updateMethodInfo == null )
			updateMethodInfo = t.GetMethod( "UpdateDefault", BindingFlags.NonPublic | BindingFlags.Instance );
		
		// we're done
		isTransitioning = false; 
		if( this.debugStates )
			Debug.Log( this.gameObject.name + ": did transition to state " + this.currentState.stateName + " from " + this.oldState.stateName );

		// process transition queue in case the enter/exit methods demanded a state
		DequeueTransitionsUntilEmpty();
	}
	
	private bool IsLegalTransition( string fromstate, string tostate ) {
		if( fromstate == "Start" && legalStates.ContainsKey( tostate ) )
			return true;
		
		if( !legalStates.ContainsKey( fromstate ) || !legalStates.ContainsKey(tostate) )
			return false;
		
		SMState f_state = (SMState)legalStates[fromstate];
		
		if( !f_state.legalTransitions.ContainsKey(tostate) )
			return false;
		
		return true;
	}
	
	void DelayedTransition() {
		isTransitioning = false; // so the request will pass ok, basically this is just a temporary semaphore here
		
		this.RequestState( timedTransition.targetState.stateName, 0, timedTransition.args );
	
		timedTransition = null;		
		transitionTimer = null;
	}
	
	protected void RequestState( string name, float time, params object[] args ) {
		// transitioning from current state to the same state always succeeds immediately
		if( currentState != null && name == this.currentState.stateName )
			return;  
		
		if( this.CanTransitionTo( name ) ) {
			if( time == 0 ) {
				// on zero time just kick off the transition immediately
				TransitionToState( name, args );
			} else {
				// set the delay timer to trigger properly
				if( IsLegalTransition( currentState.stateName, name ) ) {
					
					SMState tgtState = (SMState)legalStates[name];
					
					timedTransition = new SMStateTransition( tgtState, 0, args );
					transitionTimer = Timer.Add( time, DelayedTransition, false );
					isTransitioning = true;
					
					if( transitionTimer == null ) {
						Debug.LogError( "Wanted a timed transition but could not create timer???");
					} 
				} else {
					throw new Exception( "Attempted illegal timed transition: "+this.currentState.stateName+" => " + name );
				}
			}
		} else {
			throw new Exception( "Requested transition to state " + name + " when cannot transition? Did you mean to call DemandState() instead?" );
		}
	}
	
	protected void RequestState( string name, float time ) {
		this.RequestState( name, time, null );
	}
	
	protected void RequestState( string name ) {
		this.RequestState( name, 0, null );
	}
	
	protected void DemandState( string name, params object[] args ) {
		// Debug.Log( "in demandstate for " + name );
		
		if( !isTransitioning && transitionQueue.Count == 0 ) {
			// we're not currently transitioning into any state and the
			// queue is empty, so we can just do this right away...
			
			this.TransitionToState( name, args );
		} else {
			// ok, we must queue this -- find the proper transition
			if( legalStates.ContainsKey( name ) ) {
				SMState targetState = (SMState)legalStates[name];
				SMStateTransition trans = new SMStateTransition( targetState, 0, args );
				
				transitionQueue.Enqueue( trans );
				// Debug.Log( "transition queued: " + name );
			}
		}
	}
		
	protected void DemandState( string name ) {
		this.DemandState( name, 0, null );
	}
		
	protected void DefaultEnterState() {
		
	}
	
	protected void DefaultExitState() {
		
	}
	
	
}
