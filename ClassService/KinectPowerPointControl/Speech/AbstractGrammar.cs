using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectPowerPointControl.Speech
{
    public class AbstractGrammar: ISpeechGrammar
    {
        protected List<String> words;
        protected Dictionary<String, IList<String>> dictionary;
              
        public Boolean IsCommand(String action, String speech)
        {
            if( dictionary.ContainsKey(action) )
            {
                IList<String> list = dictionary[action];
                return IsOnList(list, speech);
            }

            return false;
        }

        protected Boolean IsOnList(IList<String> sentences, String speech)
        {
            foreach (String sentence in sentences)
            {
                if (sentence == speech)
                    return true;
            }
            return false;
        }
        
        public IList<string> GrammarWords()
        {
            return words;
        }
    }
}
