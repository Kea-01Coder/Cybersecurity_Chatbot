using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Text;

namespace Cybersecurity_ChatbotWPF.Services
{
    internal class SpeechService
    {
            private SpeechSynthesizer synth;

            public SpeechService()  //Constructor
            {
                synth = new SpeechSynthesizer();
                synth.Volume = 100;
                synth.Rate = 0;
            }

            public void Speak(string text)
            {
                try
                {
                    synth.SpeakAsync(text);  //Async for GUI (non-blocking)
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Speech error: {ex.Message}");
                }
            }
        }
}
