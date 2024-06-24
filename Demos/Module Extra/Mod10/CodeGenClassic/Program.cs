using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.IO;

namespace CodeGenClassic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var unit = CodeDom();

            var provider = new CSharpCodeProvider();
            using (var sw = new StringWriter() ) 
            {
                provider.GenerateCodeFromCompileUnit(unit, sw, new CodeGeneratorOptions());
                Console.WriteLine(sw.ToString());
            }

            Console.WriteLine("Enter to create an executable");
            Console.ReadLine();

            var compilerParameters = new CompilerParameters();
            compilerParameters.GenerateExecutable = true;
            compilerParameters.OutputAssembly = $@"{Environment.CurrentDirectory}\..\..\Dynamic.exe";
            compilerParameters.ReferencedAssemblies.Add("System.dll");

            var results = provider.CompileAssemblyFromDom(compilerParameters, unit);

            if (results.Errors.Count > 0)
            {
                foreach (CompilerError error in results.Errors)
                {
                    Console.WriteLine(error.ErrorText);
                }
                throw new InvalidOperationException("Compilation failed");
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static CodeCompileUnit CodeDom()
        {
            var compileUnit = new CodeCompileUnit();
            // Namespace
            var sub = new CodeNamespace("CodeGen.Sub");
            compileUnit.Namespaces.Add(sub);

            // using Namespaces
            sub.Imports.Add(new CodeNamespaceImport("System"));

            CreatePersonClass(sub);

            // Program class
            var prog = new CodeTypeDeclaration("Program");
            sub.Types.Add(prog);

            // Main entry point method.
            var main = new CodeEntryPointMethod();

            var pType = new CodeTypeReference("Person");
            var inst = new CodeObjectCreateExpression(pType);
            var statement1 = new CodeVariableDeclarationStatement(
                pType, "p1", inst);
            main.Statements.Add(statement1);

            var statement2 = new CodeAssignStatement(
                new CodePropertyReferenceExpression(
                    new CodeVariableReferenceExpression("p1"), "Name"),
                new CodePrimitiveExpression("Patrick"));
            main.Statements.Add(statement2);

            var statement3 = new CodeAssignStatement(
                new CodePropertyReferenceExpression(
                    new CodeVariableReferenceExpression("p1"), "Age"),
                new CodePrimitiveExpression(42));
            main.Statements.Add(statement3);

            var statement4 = new CodeMethodInvokeExpression(
                new CodeVariableReferenceExpression("p1"), "Introduce");
            main.Statements.Add(statement4);
            var console = new CodeTypeReferenceExpression("System.Console");

            // Build a Console.ReadLine
            var statement5 = new CodeMethodInvokeExpression(console, "ReadLine");
            main.Statements.Add(statement5);

            prog.Members.Add(main);

            return compileUnit;
        }

        private static void CreatePersonClass(CodeNamespace sub)
        {
            var person = new CodeTypeDeclaration("Person");
            person.Attributes = MemberAttributes.Public;
            sub.Types.Add(person);

            AutoGenProperty(person, new CodeTypeReference(typeof(string)), "Name");
            AutoGenProperty(person, new CodeTypeReference(typeof(int)), "Age");

            GenIntroduceMethod(person);
            
        }

        static void GenIntroduceMethod(CodeTypeDeclaration person)
        { 
            var intro = new CodeMemberMethod();
            intro.Name = "Introduce";
            intro.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            intro.ReturnType = new CodeTypeReference(typeof(void));
            person.Members.Add(intro);
            var console = new CodeTypeReferenceExpression("System.Console");
            var propName = new CodePropertyReferenceExpression(
                new CodeThisReferenceExpression(), "Name");
            var propAge = new CodePropertyReferenceExpression(
                new CodeThisReferenceExpression(), "Age");

            // String.Format
            var text = ("{0} ({1})");
            var stringFormat = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("System.String"),
                "Format",
                new CodePrimitiveExpression(text),
                propName,
                propAge);

            var cw = new CodeMethodInvokeExpression(console, "WriteLine", stringFormat);
            intro.Statements.Add(cw);
        }

        static void AutoGenProperty(CodeTypeDeclaration containingClass, CodeTypeReference propertyType, string propertyName)
        {
            var backingField = new CodeMemberField();
            backingField.Name = propertyName + "_";
            backingField.Type = propertyType;
            backingField.Attributes = MemberAttributes.Private;
            var backingFieldRef = new CodeVariableReferenceExpression(backingField.Name);
            containingClass.Members.Add(backingField);

            var property = new CodeMemberProperty();
            property.Name = propertyName;
            property.Type = propertyType;
            property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            property.HasGet = true;
            property.HasSet = true;
            property.GetStatements.Add(new CodeMethodReturnStatement(backingFieldRef));
            property.SetStatements.Add(new CodeAssignStatement(backingFieldRef, new CodeVariableReferenceExpression("value")));
            containingClass.Members.Add(property);
        }
    }
}
