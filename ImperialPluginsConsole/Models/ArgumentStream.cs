using System;
using System.Collections.Generic;
using System.Linq;

namespace ImperialPluginsConsole.Models
{
    public class ArgumentStream
    {
        private List<string> m_Args = new List<string>();
        private int m_Index = 0;

        public bool EndOfStream => m_Index >= m_Args.Count;

        public string ReadNext()
        {
            var val = m_Args[m_Index];
            m_Index++;
            return val;
        }

        public ArgumentStream(IEnumerable<string> args)
        {
            m_Args = args.ToList();
        }

        public bool ReadNext(out string arg)
        {
            arg = "";
            if (EndOfStream) return false;
            arg = ReadNext();
            return true;
        }
        public ArgumentParser CreateParser()
        {
            return new ArgumentParser(this);
        }

        public ArgumentList Pase(string[] dependants, string[] independants)
        {
            var dict = new ArgumentList();
            var prev = m_Index;
            m_Index = 0;
            var extra = 0;
            while (ReadNext(out var arg))
            {
                if (arg.StartsWith("-"))
                {
                    var k = arg.Substring(1);
                    if (independants.Contains(k))
                    {
                        dict[k] = "";
                    }
                    else if (dependants.Contains(k))
                    {
                        if (ReadNext(out var val))
                        {
                            dict[k] = val;
                        }
                        else
                        {
                            dict[k] = "";
                        }
                    }
                    else
                    {
                        dict[k] = "";
                    }
                }
                else
                {
                    dict[extra.ToString()] = arg;
                    extra++;
                }
            }
            m_Index = prev;
            return dict;
        }
    }
}