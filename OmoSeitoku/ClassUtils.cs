using System;
using System.Collections.Generic;

namespace OmoSeitoku
{
    public static class ClassUtils
    {

        public static IEnumerable<Type> GetDefinedClasses(string namespacePath)
        {
            foreach (System.Reflection.Assembly assm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;

                try
                {
                    types = assm.GetTypes();
                }
                catch (System.Reflection.ReflectionTypeLoadException)
                {
                    // アセンブリ内の型がまだ読み込まれていないアセンブリ内の型に依存している場合に発生する例外。無視する。
                    continue;
                }

                foreach (Type type in types)
                {
                    if (type.IsClass && !type.IsAbstract &&
                        type.Namespace == namespacePath)
                    {
                        yield return type;
                    }
                }
            }
        }
    }
}
