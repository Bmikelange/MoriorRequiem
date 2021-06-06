using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour {
    // Start is called before the first frame update
    public Font texture;
    TextToTexture text;
    public PerCharacterKerning[] ps;

    int value = 0;
    string type = "";

    public void Init(int _value, string s) {
        value = _value;
        type = s;
        Invoke("textureDice", 1.5f);
        /*        float[] p = DefaultCharacterKerning();
                ps = new PerCharacterKerning[p.Length];
                for (int x = 0; x < p.Length; x++) {
                    char cha = (char)(x + 32);
                    ps[x] = new PerCharacterKerning("" + cha, p[x]);
                }
                text = new TextToTexture(texture, 10, 10, ps, false);*/

    }

    void textureDice() {
        /*        int t = value;
                string s = "" + t;
                float sLength = s.Length;*/
        /*if (sLength == 1) {
            gameObject.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load<Texture>("dé" + s);
        }
        if (sLength == 2) {
            int textWidthPlusTrailingBuffer = text.CalcTextWidthPlusTrailingBuffer(s, 400, 2.0f);
            int textHeightPlusTrailingBuffer = text.CalcTextHeightPlusTrailingBuffer(s, 400, 2.0f);

            int posX = (400 - (textWidthPlusTrailingBuffer + 1)) - Mathf.Clamp((((int)2 - textWidthPlusTrailingBuffer) / 2), 0, 400);
            int posY = (400 - (textHeightPlusTrailingBuffer + 1)) - Mathf.Clamp((((int)2 - textHeightPlusTrailingBuffer) / 2), 0, 400);
            Texture2D resultat = text.CreateTextToTexture("" + t, posX, posY, 400, 2.0f, 0.75f);
            gameObject.GetComponent<MeshRenderer>().material.mainTexture = cropTexture(resultat, 90);
        }*/
        if (value <= 12) {
            string s= "Dice" + type;
            GetComponent<MeshRenderer>().materials[0].mainTexture = Resources.Load<Texture2D>(s + value);
            //gameObject.GetComponent<MeshRenderer>().materials[1].mainTexture = Resources.Load<Texture2D>("Dice" + value);
        }
    }

    Texture2D cropTexture(Texture2D entree, int Size) {
        Texture2D sortie = new Texture2D(entree.width - 2 * Size, entree.height - 2 * Size);
        Color[] c = entree.GetPixels(Size, Size, sortie.width, sortie.height);
        sortie.SetPixels(0, 0, sortie.width, sortie.height, c);
        sortie.Apply();
        return sortie;
    }

    private float[] DefaultCharacterKerning() {
        double[] perCharKerningDouble = new double[] {
        .201 /* */
        ,.201 /*!*/
        ,.256 /*"*/
        ,.401 /*#*/
        ,.401 /*$*/
        ,.641 /*%*/
        ,.481 /*&*/
        ,.138 /*'*/
        ,.24 /*(*/
        ,.24 /*)*/
        ,.281 /***/
        ,.421 /*+*/
        ,.201 /*,*/
        ,.24 /*-*/
        ,.201 /*.*/
        ,.201 /*/*/
        ,.401 /*0*/
        ,.353 /*1*/
        ,.401 /*2*/
        ,.401 /*3*/
        ,.401 /*4*/
        ,.401 /*5*/
        ,.401 /*6*/
        ,.401 /*7*/
        ,.401 /*8*/
        ,.401 /*9*/
        ,.201 /*:*/
        ,.201 /*;*/
        ,.421 /*<*/
        ,.421 /*=*/
        ,.421 /*>*/
        ,.401 /*?*/
        ,.731 /*@*/
        ,.481 /*A*/
        ,.481 /*B*/
        ,.52  /*C*/
        ,.481 /*D*/
        ,.481 /*E*/
        ,.44  /*F*/
        ,.561 /*G*/
        ,.52  /*H*/
        ,.201 /*I*/
        ,.36  /*J*/
        ,.481 /*K*/
        ,.401 /*L*/
        ,.6   /*M*/
        ,.52  /*N*/
        ,.561 /*O*/
        ,.481 /*P*/
        ,.561 /*Q*/
        ,.52  /*R*/
        ,.481 /*S*/
        ,.44  /*T*/
        ,.52  /*U*/
        ,.481 /*V*/
        ,.68  /*W*/
        ,.481 /*X*/
        ,.481 /*Y*/
        ,.44  /*Z*/
        ,.201 /*[*/
        ,.201 /*\*/
        ,.201 /*]*/
        ,.338 /*^*/
        ,.401 /*_*/
        ,.24  /*`*/
        ,.401 /*a*/
        ,.401 /*b*/
        ,.36  /*c*/
        ,.401 /*d*/
        ,.401 /*e*/
        ,.189 /*f*/
        ,.401 /*g*/
        ,.401 /*h*/
        ,.16  /*i*/
        ,.16  /*j*/
        ,.36  /*k*/
        ,.16  /*l*/
        ,.6   /*m*/
        ,.401 /*n*/
        ,.401 /*o*/
        ,.401 /*p*/
        ,.401 /*q*/
        ,.24  /*r*/
        ,.36  /*s*/
        ,.201 /*t*/
        ,.401 /*u*/
        ,.36  /*v*/
        ,.52  /*w*/
        ,.36  /*x*/
        ,.36  /*y*/
        ,.36  /*z*/
        ,.241 /*{*/
        ,.188 /*|*/
        ,.241 /*}*/
        ,.421 /*~*/
        };
        float[] perCharKerning = new float[perCharKerningDouble.Length];

        for (int x = 0; x < perCharKerning.Length; x++) {
            perCharKerning[x] = 0f;// (float)perCharKerningDouble[x];
        }
        return perCharKerning;
    }
}
