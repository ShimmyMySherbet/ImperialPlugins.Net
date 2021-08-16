using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPluginsConsole.Models
{
    public class ArgumentParser
    {
        private List<string> m_Dependants = new List<string>();
        private List<string> m_Independants = new List<string>();
        private ArgumentStream m_Arguments;

        public ArgumentParser(ArgumentStream st)
        {
            m_Arguments = st;
        }

        public ArgumentParser WithDependants(params string[] deps)
        {
            m_Dependants.AddRange(deps);
            return this;
        }


        public ArgumentParser WithIndependants(params string[] deps)
        {
            m_Independants.AddRange(deps);
            return this;
        }


        public ArgumentList Parse()
        {
            return m_Arguments.Pase(m_Dependants.ToArray(), m_Independants.ToArray());
        }
    }
}
