using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class JumbleCycleScript : MonoBehaviour
{

    public KMAudio Audio;
    public List<KMSelectable> keys;
    public GameObject[] dials;
    public TextMesh[] dialText;
    public TextMesh disp;

    private string[][] message = new string[2][]{ new string[100] { "ADVERTED", "ADVOCATE", "ALLOTYPE", "ALLOTTED", "BINORMAL", "BINOMIAL", "BULLHORN", "BULWARKS", "CONNECTS", "CONQUERS", "COMMANDO", "COMPILER", "DECEIVED", "DECIMATE", "DISPATCH", "DISCRETE", "ENCRYPTS", "ENCODING", "EQUATORS", "EQUALISE", "FINALISE", "FINNICKY", "FORMULAE", "FORTUNES", "GARRISON", "GARNERED", "GATEPOST", "GATEWAYS", "HOTLINKS", "HOTHEADS", "HUNTRESS", "HUNDREDS", "INCOMING", "INDIRECT", "ILLUSORY", "ILLUDING", "JOURNEYS", "JOUSTING", "JUNKYARD", "JUNCTURE", "KILOVOLT", "KILOBYTE", "KNOCKING", "KNOWABLE", "LANGUAGE", "LANDMARK", "LINKWORK", "LINGERED", "MONOMIAL", "MONOLITH", "MULTITON", "MULCTING", "NANOWATT", "NANOBOTS", "NUMEROUS", "NUMERATE", "ORDERING", "ORDINALS", "OBSTRUCT", "OBSTACLE", "PROPHASE", "PROPHECY", "POSTSYNC", "POSITRON", "QUARTILE", "QUARTICS", "QUIRKISH", "QUITTERS", "REVERSED", "REVEALED", "RELAYING", "RELATIVE", "STANZAIC", "STANDOUT", "STOCKADE", "STOCCATA", "TRIGONAL", "TRICKIER", "TOMOGRAM", "TOMAHAWK", "UNDERWAY", "UNDOINGS", "ULTERIOR", "ULTRAHOT", "VENOMOUS", "VENDETTA", "VOLITION", "VOLUMING", "WEAKENED", "WEAPONED", "WHATNESS", "WHATSITS", "YEARLONG", "YEARNING", "YOKOZUNA", "YOURSELF", "ZYGOMATA", "ZYGOTENE", "ZYMOLOGY", "ZYMOGENE" },
                                                  new string[100] { "ULTRAHOT", "DECEIVED", "QUITTERS", "QUIRKISH", "RELAYING", "ADVERTED", "WEAPONED", "ZYMOLOGY", "JUNCTURE", "YOURSELF", "NUMERATE", "HUNTRESS", "UNDOINGS", "ADVOCATE", "HUNDREDS", "ALLOTYPE", "STANDOUT", "MONOMIAL", "YOKOZUNA", "COMPILER", "TRIGONAL", "LANGUAGE", "ILLUSORY", "GATEWAYS", "QUARTICS", "TOMAHAWK", "WHATNESS", "POSTSYNC", "POSITRON", "REVEALED", "ZYGOTENE", "JOURNEYS", "NUMEROUS", "ORDERING", "VENDETTA", "WHATSITS", "PROPHASE", "BINOMIAL", "QUARTILE", "INDIRECT", "DISPATCH", "BINORMAL", "DECIMATE", "REVERSED", "VENOMOUS", "BULWARKS", "STOCKADE", "KNOWABLE", "JOUSTING", "ZYMOGENE", "JUNKYARD", "HOTLINKS", "TRICKIER", "STANZAIC", "HOTHEADS", "GARNERED", "LANDMARK", "KILOVOLT", "EQUALISE", "PROPHECY", "MONOLITH", "WEAKENED", "KILOBYTE", "DISCRETE", "ENCODING", "ILLUDING", "EQUATORS", "ORDINALS", "CONNECTS", "FINALISE", "FORTUNES", "COMMANDO", "INCOMING", "OBSTACLE", "FORMULAE", "OBSTRUCT", "ULTERIOR", "CONQUERS", "GATEPOST", "MULCTING", "KNOCKING", "GARRISON", "MULTITON", "YEARLONG", "LINGERED", "TOMOGRAM", "ENCRYPTS", "BULLHORN", "ALLOTTED", "UNDERWAY", "ZYGOMATA", "YEARNING", "STOCCATA", "FINNICKY", "LINKWORK", "VOLITION", "NANOBOTS", "NANOWATT", "RELATIVE", "VOLUMING" } };
    private string[] pigpens = new string[4] { "ASCUIVGT", "BKFOHQDM", "JWLYRZPX", "EN" };
    private string[][] ciphertext = new string[2][] { new string[6], new string[6]};
    private string[][] digraphs = new string[2][] { new string[4], new string[4] };
    private string answer;
    private int[][] rot = new int[2][] { new int[8], new int[8] };
    private readonly string[] ciphers = new string[4] { "Caesar", "Affine", "Pipgen", "Playfair" };
    private int pressCount;
    private bool moduleSolved;

    //Logging
    static int moduleCounter = 1;
    int moduleID;

    private void Awake()
    {
        moduleID = moduleCounter++;
        foreach (KMSelectable key in keys)
        {
            int k = keys.IndexOf(key);
            key.OnInteract += delegate () { KeyPress(k); return false; };
        }
    }

    void Start()
    {
        Reset();
    }

    private void KeyPress(int k)
    {
        keys[k].AddInteractionPunch(0.125f);
        if (moduleSolved == false)
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            if (k == 26)
            {
                pressCount = 0;
                answer = string.Empty;
            }
            else
            {
                pressCount++;
                answer = answer + "QWERTYUIOPASDFGHJKLZXCVBNM"[k];
            }
            disp.text = answer;
            if (pressCount == 8)
            {
                if (answer == ciphertext[1][4])
                {
                    moduleSolved = true;
                    Audio.PlaySoundAtTransform("InputCorrect", transform);
                    disp.color = new Color32(0, 255, 0, 255);
                }
                else
                {
                    GetComponent<KMBombModule>().HandleStrike();
                    disp.color = new Color32(255, 0, 0, 255);
                    Debug.LogFormat("[Jumble Cycle #{0}]The submitted response was {1}: Resetting", moduleID, answer);
                }
                Reset();
            }
        }
    }

    private void Reset()
    {

        StopAllCoroutines();
        if (moduleSolved == false)
        {
            pressCount = 0;
            answer = string.Empty;
            int fix = Random.Range(0, 4);
            int r = Random.Range(0, 100);
            int[] method = new int[8];
            string[] roh = new string[8];
            List<string>[] playkey = new List<string>[2] { new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" }, new List<string> { } };
            string[][] playtable = new string[5][] { new string[5], new string[5], new string[5], new string[5], new string[5] };
            List<string>[][] ciph = new List<string>[2][] { new List<string>[5] { new List<string> { }, new List<string> { }, new List<string> { }, new List<string> { }, new List<string> { } }, new List<string>[5] { new List<string> { }, new List<string> { }, new List<string> { }, new List<string> { }, new List<string> { } } };
            List<int> usedvals = new List<int> { };
            List<string> logmethod = new List<string> { };
            for (int i = 0; i < 8; i++)
            {
                if (i == 2 * fix || i == 2 * fix + 1)
                {
                    method[i] = 3;
                    dialText[i].color = new Color32(0, 255, 255, 255);
                }
                else
                {
                    method[i] = Random.Range(0, 3);
                    switch (method[i])
                    {
                        case 0:
                            dialText[i].color = new Color32(255, 255, 255, 255);
                            break;
                        case 1:
                            dialText[i].color = new Color32(255, 255, 0, 255);
                            break;
                        case 2:
                            dialText[i].color = new Color32(255, 0, 255, 255);
                            break;
                    }
                }
                logmethod.Add(ciphers[method[i]]);
            }
            for (int i = 0; i < 8; i++)
            {
                dialText[i].text = string.Empty;
                rot[1][i] = rot[0][i];
                rot[0][i] = Random.Range(0, 8);
                if(rot[0][0] == 6 && method[0] == 1)
                {
                    rot[0][0] = 7;
                }
            }
            for(int i = 0; i < 7; i++)
            {
                while(usedvals.Contains((8 * rot[0][i] + rot[0][i + 1]) % 26) || (8 * rot[0][i] + rot[0][i + 1]) % 26 == 23 || (rot[0][i + 1] == 6 && method[i + 1] == 1))
                {
                    rot[0][i + 1] = Random.Range(0, 8);
                }
                roh[i] = rot[0][i].ToString();
                usedvals.Add((8 * rot[0][i] + rot[0][i + 1]) % 26);
                playkey[1].Add(playkey[0][(8 * rot[0][i] + rot[0][i + 1]) % 26]);
            }
            roh[7] = rot[0][7].ToString();
            playkey[0].Remove("X");
            for (int i = 0; i < 25; i++)
            {
                if (i < 7)
                {
                    playkey[0].Remove(playkey[1][i]);
                }
                else
                {
                    playkey[1].Add(playkey[0][i - 7]);
                }
                playtable[Mathf.FloorToInt(i / 5)][i % 5] = playkey[1][i];
            }
            for (int i = 0; i < 4; i++)
            {
                if (message[0][r][2 * i] == message[0][r][2 * i + 1])
                {
                    digraphs[0][i] = message[0][r][2 * i].ToString() + "Z";
                }
                else
                {
                    digraphs[0][i] = message[0][r][2 * i].ToString() + message[0][r][2 * i + 1].ToString();
                }
                if (message[1][r][2 * i] == message[1][r][2 * i + 1])
                {
                    digraphs[1][i] = message[1][r][2 * i].ToString() + "Z";
                }
                else
                {
                    digraphs[1][i] = message[1][r][2 * i].ToString() + message[1][r][2 * i + 1].ToString();
                }
            }
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    if (method[i] == 0)
                    {
                        ciph[j][0].Add("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[("ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(message[j][r][i]) + rot[0][i]) % 26].ToString());
                    }
                    else
                    {
                        ciph[j][0].Add("/");
                    }
                    if (method[i] == 1)
                    {
                        ciph[j][1].Add("ZABCDEFGHIJKLMNOPQRSTUVWXY"[("ZABCDEFGHIJKLMNOPQRSTUVWXY".IndexOf(message[j][r][i]) * (2 * rot[0][i] + 1)) % 26].ToString());
                    }
                    else
                    {
                        ciph[j][1].Add("/");
                    }
                }
                for(int j = 0; j < 4; j++)
                {
                    for(int k = 0; k < 2; k++)
                    {
                        if (pigpens[j].Contains(message[k][r][i].ToString()))
                        {
                            if (method[i] == 2)
                            {
                                if (j == 3)
                                {
                                    ciph[k][2].Add(pigpens[3][(pigpens[3].IndexOf(message[k][r][i]) + rot[0][i]) % 2].ToString());
                                }
                                else
                                {
                                    ciph[k][2].Add(pigpens[j % 3][(pigpens[j].IndexOf(message[k][r][i]) + rot[0][i]) % 8].ToString());
                                }
                            }
                            else
                            {
                                ciph[k][2].Add("/");
                            }
                        }
                        if (i == 7)
                        {
                            if (method[2 * j + k] == 3)
                            {
                                int[] y = new int[2] { playkey[1].IndexOf(digraphs[k][j][0].ToString()) % 5, playkey[1].IndexOf(digraphs[k][j][1].ToString()) % 5 };
                                int[] x = new int[2] { Mathf.FloorToInt(playkey[1].IndexOf(digraphs[k][j][0].ToString()) / 5), Mathf.FloorToInt(playkey[1].IndexOf(digraphs[k][j][1].ToString()) / 5) };
                                if (x[0] == x[1])
                                {
                                    ciph[k][3].Add(playtable[x[0]][(y[0] + 1) % 5]);
                                    ciph[k][3].Add(playtable[x[1]][(y[1] + 1) % 5]);
                                }
                                else if (y[0] == y[1])
                                {
                                    ciph[k][3].Add(playtable[(x[0] + 1) % 5][y[0]]);
                                    ciph[k][3].Add(playtable[(x[1] + 1) % 5][y[1]]);
                                }
                                else
                                {
                                    ciph[k][3].Add(playtable[x[0]][y[1]]);
                                    ciph[k][3].Add(playtable[x[1]][y[0]]);
                                }
                            }
                            else
                            {
                                ciph[k][3].Add("//");
                            }
                        }
                    }
                }

            }
            for(int i = 0; i < 2; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    ciphertext[i][j] = string.Join(string.Empty, ciph[i][j].ToArray());
                }
            }
            for(int i = 0; i < 2; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    ciph[i][4].Add(ciphertext[i][method[j]][j].ToString());
                }
                ciphertext[i][4] = string.Join(string.Empty, ciph[i][4].ToArray());
            }
            Debug.LogFormat("[Jumble Cycle #{0}]The Jumble encrypted message was {1}", moduleID, ciphertext[0][4]);
            Debug.LogFormat("[Jumble Cycle #{0}]The dial rotations were {1}", moduleID, string.Join(", ", roh));
            Debug.LogFormat("[Jumble Cycle #{0}]The dial ciphers were {1}", moduleID, string.Join(", ", logmethod.ToArray()));
            Debug.LogFormat("[Jumble Cycle #{0}]The Caesar encrypted message was {1}", moduleID, ciphertext[0][0]);
            Debug.LogFormat("[Jumble Cycle #{0}]The Affine encrypted message was {1}", moduleID, ciphertext[0][1]);
            Debug.LogFormat("[Jumble Cycle #{0}]The Pigpen encrypted message was {1}", moduleID, ciphertext[0][2]);
            Debug.LogFormat("[Jumble Cycle #{0}]The Playfair encrypted message was {1}", moduleID, ciphertext[0][3]);
            string logsquare;
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                    logsquare = "The keysquare used for Playfair encryption was:\n[Jumble Cycle #" + moduleID + "]";
                else
                    logsquare = string.Empty;
                Debug.LogFormat("[Jumble Cycle #{0}] {2} {1}", moduleID, string.Join(" ", playtable[i]), logsquare);
            }
            Debug.LogFormat("[Jumble Cycle #{0}]The deciphered message was {1}", moduleID, message[0][r]);
            Debug.LogFormat("[Jumble Cycle #{0}]The response word was {1}", moduleID, message[1][r]);
            Debug.LogFormat("[Jumble Cycle #{0}]The Caesar encrypted response was {1}", moduleID, ciphertext[1][0]);
            Debug.LogFormat("[Jumble Cycle #{0}]The Affine encrypted response was {1}", moduleID, ciphertext[1][1]);
            Debug.LogFormat("[Jumble Cycle #{0}]The Pigpen encrypted response was {1}", moduleID, ciphertext[1][2]);
            Debug.LogFormat("[Jumble Cycle #{0}]The Playfair encrypted response was {1}", moduleID, ciphertext[1][3]);
            Debug.LogFormat("[Jumble Cycle #{0}]The correct response was {1}", moduleID, ciphertext[1][4]);
        }
        StartCoroutine(DialSet());
    }

    private IEnumerator DialSet()
    {
        int[] spin = new int[8];
        bool[] set = new bool[8];
        for (int i = 0; i < 8; i++)
        {
            if (moduleSolved == false)
            {
                spin[i] = rot[0][i] - rot[1][i];
            }
            else
            {
                spin[i] = -rot[0][i];
            }
            if (spin[i] < 0)
            {
                spin[i] += 8;
            }
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (spin[j] == 0)
                {
                    if (set[j] == false)
                    {
                        set[j] = true;
                        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, transform);
                        if (moduleSolved == false)
                        {
                            dialText[j].text = ciphertext[0][4][j].ToString();
                        }
                        else
                        {
                            switch (j)
                            {
                                case 0:
                                    dialText[j].text = "V";
                                    break;
                                case 2:
                                    dialText[j].text = "R";
                                    break;
                                case 3:
                                    dialText[j].text = "Y";
                                    break;
                                case 4:
                                    dialText[j].text = "N";
                                    break;
                                case 5:
                                    dialText[j].text = "I";
                                    break;
                                case 6:
                                    dialText[j].text = "C";
                                    break;
                                default:
                                    dialText[j].text = "E";
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    dials[j].transform.localEulerAngles += new Vector3(0, 0, 45);
                    spin[j]--;
                }
            }
            if (i < 7)
            {
                yield return new WaitForSeconds(0.25f);
            }
        }
        if (moduleSolved == true)
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            GetComponent<KMBombModule>().HandlePass();
        }
        disp.text = string.Empty;
        disp.color = new Color32(255, 255, 255, 255);
        yield return null;
    }
#pragma warning disable 414
    private string TwitchHelpMessage = "!{0} QWERTYUI [Inputs letters] | !{0} cancel [Deletes inputs]";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {

        if (command.ToLowerInvariant() == "cancel")
        {
            KeyPress(26);
            yield return null;
        }
        else
        {
            command = command.ToUpperInvariant();
            var word = Regex.Match(command, @"^\s*([A-Z\-]+)\s*$");
            if (!word.Success)
            {
                yield break;
            }
            command = command.Replace(" ", string.Empty);
            foreach (char letter in command)
            {
                KeyPress("QWERTYUIOPASDFGHJKLZXCVBNM".IndexOf(letter));
                yield return new WaitForSeconds(0.125f);
            }
            yield return null;
        }
    }
}
