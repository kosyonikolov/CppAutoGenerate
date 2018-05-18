using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarbageCreator
{
    public class BasicClassCreator : IClassCreator
    {
        private struct GeneratedField
        {
            public FieldInfo info;
            public string getter, setter;
            public string getterDeclaration, setterDeclaration;
        }

        private AccessorCreatorStore store = new AccessorCreatorStore();
        private IList<GeneratedField> generatedFields = new List<GeneratedField>();
        private string className = null;

        public BasicClassCreator()
        {
            store.Add("char*", new StringAccessorCreator());
        }

        public GeneratedClass GenerateClass(string className, IList<FieldInfo> fields)
        {
            GeneratedClass result = new GeneratedClass();

            GenerateFields(fields);
            this.className = className;

            result.headerFile = GenerateHeader();
            result.cppFile = GenerateCpp();
            
            return result;
        }

        private string GenerateHeader()
        {
            StringBuilder fieldDeclartionBuilder = new StringBuilder();
            StringBuilder accessorDeclarationBuilder = new StringBuilder();
            foreach (var field in generatedFields)
            {
                // стринговете да са 0 по начало
                if (field.info.type == "char*") fieldDeclartionBuilder.AppendFormat("\tchar* {0} = 0;\r\n", field.info.name);
                else fieldDeclartionBuilder.AppendFormat("\t{0} {1};\r\n", field.info.type, field.info.name);

                accessorDeclarationBuilder.AppendFormat("\t{0};\r\n", field.getterDeclaration);
                accessorDeclarationBuilder.AppendFormat("\t{0};\r\n", field.setterDeclaration);
            }

            return string.Format(R.basicClassHeader, className, fieldDeclartionBuilder, accessorDeclarationBuilder);
        }

        private string GenerateCpp()
        {
            StringBuilder copyFunctionBuilder = new StringBuilder();
            StringBuilder destructorBuilder = new StringBuilder();
            StringBuilder accessorBuilder = new StringBuilder();

            foreach (var genField in generatedFields)
            {
                FieldInfo field = genField.info;

                if (field.type == "char*")
                {
                    string charSetter = genField.setterDeclaration;
                    charSetter = charSetter.Substring(charSetter.IndexOf(' ') + 1);
                    charSetter = charSetter.Substring(0, charSetter.IndexOf('('));
                    copyFunctionBuilder.AppendFormat("\t{0}(other.{1});\r\n", charSetter, field.name);
                    destructorBuilder.AppendFormat("\tif ({0}) delete[] {0};\r\n", field.name);
                }
                else copyFunctionBuilder.AppendFormat("\tthis->{0} = other.{0};\r\n", field.name);

                string getter = AddClassToFunction(genField.getter);
                accessorBuilder.AppendLine(getter);
                accessorBuilder.AppendLine();

                string setter = AddClassToFunction(genField.setter);
                accessorBuilder.AppendLine(setter);
                accessorBuilder.AppendLine();
            }

            return string.Format(R.basicClassCpp, className,
                copyFunctionBuilder,
                destructorBuilder,
                accessorBuilder);
        }

        private void GenerateFields(IList<FieldInfo> fields)
        {
            generatedFields.Clear();
            foreach (var fieldInfo in fields)
            {
                generatedFields.Add(GenerateField(fieldInfo));
            }
        }

        private GeneratedField GenerateField(FieldInfo info)
        {
            GeneratedField result = new GeneratedField();
            result.info = info;

            IAccessorCreator creator = store[info.type];
            result.getter = creator.MakeGetter(info);
            result.setter = creator.MakeSetter(info);

            result.getterDeclaration = result.getter.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries)[0];
            result.setterDeclaration = result.setter.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries)[0];

            return result;
        }

        private string AddClassToFunction(string declaration)
        {
            int insertIdx = declaration.IndexOf('(');
            while (declaration[insertIdx] != ' ') insertIdx--;
            return declaration.Insert(insertIdx + 1, className + "::");
        }
    }
}
