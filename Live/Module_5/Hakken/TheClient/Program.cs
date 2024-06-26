using System.Reflection;

namespace TheClient;

internal class Program
{
    static void Main(string[] args)
    {
        //Person p1 = new Person { FirstName = "Jan", LastName = "Hendriks", Age = 35 };
        //p1.Introduce();

        Assembly asm = Assembly.LoadFile(@"E:\DotnetEssentials\dne-06-2024\Live\Module_5\Hakken\MyLib\Dist\MyLib.dll");
        Console.WriteLine(asm.FullName);
        //WatZitErin(asm);
        KanIkErWatMee(asm);
        Console.ReadLine();
    }

    private static void KanIkErWatMee(Assembly asm)
    {
        Type? t = asm.GetType("MyLib.Person");
        object? o1 = Activator.CreateInstance(t);

        PropertyInfo? fn = t.GetProperty("FirstName");
        var ln = t.GetProperty("LastName");
        var age = t.GetProperty("Age");

        fn?.SetValue(o1, "Peter");
        ln?.SetValue(o1, "Janssen");
        age?.SetValue(o1, 42);

        FieldInfo? fi = t.GetField("_age", BindingFlags.Instance | BindingFlags.NonPublic);
        fi.SetValue(o1, -42);

        MethodInfo? mi = t.GetMethod("Introduce");
        mi?.Invoke(o1, []);

        dynamic? o2 = Activator.CreateInstance(t);
        o2.FirstName = "Marieke";
        o2.LastName = "Otten";
        o2.Age = 19;

        o2.Introduce();

        Console.WriteLine(t.FullName);
    }

    private static void WatZitErin(Assembly asm)
    {
        foreach(Type t in asm.GetTypes())
        {
            Console.WriteLine(t.FullName);
            foreach(MemberInfo m in  t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                Console.WriteLine(m.Name);
            }
        }
    }
}
