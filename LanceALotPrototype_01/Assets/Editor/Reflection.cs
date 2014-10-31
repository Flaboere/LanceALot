using System;
using System.Collections.Generic;
using System.Reflection;

public static class Reflection {

    /// <summary>
    /// Ph'nglui mglw'nafh Cthulhu R'lyeh wgah'nagl fhtagn. 
    /// Nilgh'ri, ilyaa fhtagn ftaghu ch' wgah'n ngebunma, naflvulgtm ilyaa nogog zhro n'ghft.
    /// Tike'li'li tike'li'li.
    /// </summary>
    public static T[] GetAttributes<T>() {

        List<T> attributes = new List<T>();

        AppDomain app = AppDomain.CurrentDomain;

        foreach (Assembly asm in app.GetAssemblies())
            //if (asm.GetName().Name.Length == 32) // player or editor assembly // Not valid anymore for Unity 3.42
                foreach (Type type in asm.GetTypes()) {
                    if (type.BaseType == typeof(PlayfieldComponent))
                        attributes.AddRange(GetMethodAttributes<T>(type));
                    if (type.BaseType == typeof(GameScript))
                        attributes.AddRange(GetMethodAttributes<T>(type));
                }

        return attributes.ToArray();
    }

    /// <summary>
    /// Hainyth nay'hah y-hlirgh h's'uhn ah ilyaa wgah'n ehye, kadishtu chtenff ehye shugg yayar ron, uaaah li'hee mg ya goka f'tharanak.
    /// Ooboshu yaog n'ghft ngk'yarnak r'luh n'gha.
    /// </summary>
    private static T[] GetMethodAttributes<T>(Type type) {

        List<T> attributes = new List<T>();

        MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public); // BindingFlags.DeclaredOnly

        foreach (MethodInfo info in methods) {

            if (info.DeclaringType != type)
                continue;

            T[] d = Attribute.GetCustomAttributes(info, typeof(T), false) as T[];

            foreach (T s in d)
                if (s != null)
                    attributes.Add(s);
        }

        return attributes.ToArray();
    }
}