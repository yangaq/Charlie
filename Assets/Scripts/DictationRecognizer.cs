using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;

public class DictationRecognizer : MonoBehaviour {

    [System.Serializable]
    public class DictationCompleteEvent : UnityEvent<string> { }

    [System.Serializable]
    public class DictationErrorEvent : UnityEvent<string, int> { }

    [System.Serializable]
    public class DictationHypothesisEvent : UnityEvent<string> { }

    [System.Serializable]
    public class DictationResultEvent : UnityEvent<string, string> { }

    private UnityEngine.Windows.Speech.DictationRecognizer dictationRecognizer;

    public DictationCompleteEvent dictationCompleteEvent;
    public DictationErrorEvent dictationErrorEvent;
    public DictationHypothesisEvent dictationHypothesisEvent;
    public DictationResultEvent dictationResultEvent;

    [Tooltip("The time length in seconds before dictation recognizer session " +
        "ends due to lack of audio input.")]
    public float autoSilenceTimeoutSeconds = 20.0f;

    [Tooltip("The time length in seconds before dictation recognizer session " +
        "ends due to lack of audio input in case there was no audio heard in " +
        "the current session.")]
    public float initialSilenceTimeoutSeconds = 5.0f;

	// Use this for initialization
	void Start () {
        dictationRecognizer = new UnityEngine.Windows.Speech.DictationRecognizer();
        dictationRecognizer.DictationResult += OnDictationResult;
        dictationRecognizer.DictationHypothesis += OnDictationHypothesis;
        dictationRecognizer.DictationComplete += OnDictationComplete;
        dictationRecognizer.DictationError += OnDictationError;
        dictationRecognizer.AutoSilenceTimeoutSeconds = autoSilenceTimeoutSeconds;
        dictationRecognizer.InitialSilenceTimeoutSeconds = initialSilenceTimeoutSeconds;
        dictationRecognizer.Start();
	}

    private void OnDictationError(string error, int hresult)
    {
        Debug.Log($"OnDictationError (error: {error}, result: {hresult})");
        dictationErrorEvent.Invoke(error, hresult);
    }

    private void OnDictationComplete(DictationCompletionCause cause)
    {
        Debug.Log($"OnDictationComplete (cause: {cause.ToString()})");
        dictationCompleteEvent.Invoke(cause.ToString());
        dictationRecognizer.Start();
    }

    private void OnDictationHypothesis(string text)
    {
        Debug.Log($"OnDictationHypothesis (text: {text})");
        dictationHypothesisEvent.Invoke(text);
    }

    private void OnDictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log($"OnDictationResult (text: {text}, confidence: {confidence.ToString()})");
        dictationResultEvent.Invoke(text, confidence.ToString());
        dictationRecognizer.Stop();
    }
}
