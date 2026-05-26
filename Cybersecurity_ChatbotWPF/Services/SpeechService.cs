using System;
using System.Collections.Generic;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Windows.Threading;

namespace Cybersecurity_ChatbotWPF.Services
{
        public class SpeechServices
        {
            private SpeechSynthesizer synthesizer;
            private SpeechRecognitionEngine recognizer;
            private Dispatcher uiDispatcher;
            private bool isListening;

            public event Action<string> SpeechRecognized;

            public SpeechServices(Dispatcher dispatcher)
            {
                uiDispatcher = dispatcher;
                InitializeSynthesizer();
                InitializeRecognizer();
            }

            private void InitializeSynthesizer()
            {
                try
                {
                    synthesizer = new SpeechSynthesizer();
                    synthesizer.Volume = 100;
                    synthesizer.Rate = 0;
                    // Optional: Change voice
                    // synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Speech synthesizer error: {ex.Message}");
                }
            }

            private void InitializeRecognizer()
            {
                try
                {
                    recognizer = new SpeechRecognitionEngine();
                    recognizer.SetInputToDefaultAudioDevice();

                    // Add common cybersecurity commands
                    Choices commands = new Choices(
                        "password", "phishing", "scam", "privacy",
                        "help", "exit", "quit", "bye",
                        "tell me more", "another tip", "explain more",
                        "i am worried", "i'm worried", "i am curious", "i'm curious",
                        "safe browsing", "malware", "virus", "hacking"
                    );

                    GrammarBuilder gb = new GrammarBuilder(commands);
                    Grammar grammar = new Grammar(gb);
                    recognizer.LoadGrammar(grammar);

                    recognizer.SpeechRecognized += (s, e) =>
                    {
                        string command = e.Result.Text;
                        uiDispatcher.Invoke(() => SpeechRecognized?.Invoke(command));
                    };
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Speech recognition unavailable: {ex.Message}");
                    recognizer = null;
                }
            }

            public void Speak(string text)
            {
                try
                {
                    if (synthesizer != null && !string.IsNullOrWhiteSpace(text))
                    {
                        synthesizer.SpeakAsync(text);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Speech failed: {ex.Message}");
                }
            }

            public void StartListening()
            {
                try
                {
                    if (recognizer != null && !isListening)
                    {
                        recognizer.RecognizeAsync(RecognizeMode.Single);
                        isListening = true;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to start listening: {ex.Message}");
                }
            }

            public void StopListening()
            {
                try
                {
                    if (recognizer != null && isListening)
                    {
                        recognizer.RecognizeAsyncStop();
                        isListening = false;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to stop listening: {ex.Message}");
                }
            }

            public bool IsSpeechRecognitionAvailable()
            {
                return recognizer != null;
            }

            public bool IsSpeechSynthesisAvailable()
            {
                return synthesizer != null;
            }
        }
}

          
