using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectPowerPointControl.Speech
{
    public interface ISpeechGrammar
    {
        IList<String> GrammarWords();
        Boolean IsCommand(String action, String speech);
    }
}
