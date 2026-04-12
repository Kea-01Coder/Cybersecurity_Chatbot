using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace ConsoleApp3
{
    internal class Speech
    {
        
        private static SpeechSynthesizer synth = new SpeechSynthesizer();

        static Speech()
        {
            synth.Volume = 100;
            synth.Rate = 0;
        }

        public static void SpeakAndWait(string text)
        {
            synth.Speak(text); // this waits until speech finishes
        }
    }

}
