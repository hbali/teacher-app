using UnityEngine;

namespace I2.Loc
{
    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif

    public class LocalizeTarget_UnityStd_Texture : LocalizeTarget<GUITexture>
    {
        static LocalizeTarget_UnityStd_Texture() { AutoRegister(); }
        [RuntimeInitializeOnLoadMethod]
        static void AutoRegister()
        {
            LocalizationManager.RegisterTarget(new LocalizeTarget_UnityStd_Texture());
        }
        public override string GetName() { return "GUITexture"; }
        public override bool CanUseSecondaryTerm() { return false; }
        public override bool AllowMainTermToBeRTL() { return false; }
        public override bool AllowSecondTermToBeRTL() { return false; }

        public override void GetFinalTerms ( Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
        {
            var mTarget = GetTarget(cmp);
            primaryTerm = mTarget.texture ? mTarget.texture.name : string.Empty;
            secondaryTerm = null;
        }


        public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
        {
            var mTarget = GetTarget(cmp);
            Texture Old = mTarget.texture;
            if (Old != null && Old.name != mainTranslation)
                mTarget.texture = cmp.FindTranslatedObject<Texture>(mainTranslation);

            // If the old value is not in the translatedObjects, then unload it as it most likely was loaded from Resources
            //if (!HasTranslatedObject(Old))
            //	Resources.UnloadAsset(Old);
        }
    }

    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif

    public class LocalizeTarget_UnityStd_AudioSource : LocalizeTarget<AudioSource>
    {
        static LocalizeTarget_UnityStd_AudioSource() { AutoRegister(); }
        [RuntimeInitializeOnLoadMethod]
        static void AutoRegister()
        {
            LocalizationManager.RegisterTarget(new LocalizeTarget_UnityStd_AudioSource());
        }
        public override string GetName() { return "AudioSource"; }
        public override bool CanUseSecondaryTerm() { return false; }
        public override bool AllowMainTermToBeRTL() { return false; }
        public override bool AllowSecondTermToBeRTL() { return false; }

        public override void GetFinalTerms ( Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
        {
            var mTarget = GetTarget(cmp);
            primaryTerm = mTarget.clip ? mTarget.clip.name : string.Empty;
            secondaryTerm = null;
        }


        public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
        {
            var mTarget = GetTarget(cmp);
            bool bIsPlaying = mTarget.isPlaying;
            AudioClip OldClip = mTarget.clip;
            AudioClip NewClip = cmp.FindTranslatedObject<AudioClip>(mainTranslation);
            if (OldClip != NewClip)
                mTarget.clip = NewClip;

            if (bIsPlaying && mTarget.clip)
                mTarget.Play();

            // If the old clip is not in the translatedObjects, then unload it as it most likely was loaded from Resources
            //if (!HasTranslatedObject(OldClip))
            //	Resources.UnloadAsset(OldClip);
        }
    }

    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif

    public class LocalizeTarget_UnityStd_SpriteRenderer : LocalizeTarget<SpriteRenderer>
    {
        static LocalizeTarget_UnityStd_SpriteRenderer() { AutoRegister(); }
        [RuntimeInitializeOnLoadMethod]
        static void AutoRegister()
        {
            LocalizationManager.RegisterTarget(new LocalizeTarget_UnityStd_SpriteRenderer());
        }
        public override string GetName() { return "SpriteRenderer"; }
        public override bool CanUseSecondaryTerm() { return false; }
        public override bool AllowMainTermToBeRTL() { return false; }
        public override bool AllowSecondTermToBeRTL() { return false; }

        public override void GetFinalTerms ( Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
        {
            var mTarget = GetTarget(cmp);
            primaryTerm = mTarget.sprite != null ? mTarget.sprite.name : string.Empty;
            secondaryTerm = null;
        }

        public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
        {
            var mTarget = GetTarget(cmp);
            Sprite Old = mTarget.sprite;
            if (Old == null || Old.name != mainTranslation)
                mTarget.sprite = cmp.FindTranslatedObject<Sprite>(mainTranslation);

            // If the old value is not in the translatedObjects, then unload it as it most likely was loaded from Resources
            //if (!HasTranslatedObject(Old))
            //	Resources.UnloadAsset(Old);
        }
    }

    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif

    public class LocalizeTarget_UnityStd_Prefab : LocalizeTarget<GameObject>
    {
        static LocalizeTarget_UnityStd_Prefab() { AutoRegister(); }
        [RuntimeInitializeOnLoadMethod]
        static void AutoRegister()
        {
            LocalizationManager.RegisterTarget(new LocalizeTarget_UnityStd_Prefab());
        }
        public override string GetName() { return "Prefab"; }
        public override bool CanUseSecondaryTerm() { return false; }
        public override bool AllowMainTermToBeRTL() { return false; }
        public override bool AllowSecondTermToBeRTL() { return false; }

        public override void GetFinalTerms ( Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
        {
            var mTarget = GetTarget(cmp);
            primaryTerm = mTarget.name;
            secondaryTerm = null;
        }

        public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
        {
            var mTarget = GetTarget(cmp);
            if (mTarget && mTarget.name == mainTranslation)
                return;

            GameObject current = mTarget;
            GameObject NewPrefab = cmp.FindTranslatedObject<GameObject>(mainTranslation);
            if (NewPrefab)
            {
                cmp.mTarget = Object.Instantiate(NewPrefab);
                Transform mNew = mTarget.transform;
                Transform bBase = (current ? current.transform : NewPrefab.transform);

                mNew.SetParent(cmp.transform);
                mNew.localScale = bBase.localScale;
                mNew.localRotation = bBase.localRotation;
                mNew.localPosition = bBase.localPosition;
            }

            if (current)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                    Object.Destroy(current);
                else
                    Object.DestroyImmediate(current);
#else
					Object.Destroy (current);
#endif
            }
        }

        public override bool CanLocalize(Localize cmp)
        {
            return cmp.transform.childCount > 0;
        }
    }

    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif

    public class LocalizeTarget_UnityStd_GUIText : LocalizeTarget<GUIText>
    {
        static LocalizeTarget_UnityStd_GUIText() { AutoRegister(); }
        [RuntimeInitializeOnLoadMethod]
        static void AutoRegister()
        {
            LocalizationManager.RegisterTarget(new LocalizeTarget_UnityStd_GUIText());
        }
        public TextAlignment mAlignment_RTL = TextAlignment.Right;
        public TextAlignment mAlignment_LTR = TextAlignment.Left;
        public bool mAlignmentWasRTL;
        public bool mInitializeAlignment = true;
        
        public override string GetName() { return "GUIText"; }
        public override bool CanUseSecondaryTerm() { return true; }
        public override bool AllowMainTermToBeRTL() { return true; }
        public override bool AllowSecondTermToBeRTL() { return false; }

        public override void GetFinalTerms ( Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
        {
            var mTarget = GetTarget(cmp);
            primaryTerm = mTarget.text;
            secondaryTerm = (string.IsNullOrEmpty(Secondary) && mTarget.font != null) ? mTarget.font.name : null;
        }

        public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
        {
            var mTarget = GetTarget(cmp);

            //--[ Localize Font Object ]----------
            Font newFont = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
            if (newFont != null && mTarget.font != newFont)
                mTarget.font = newFont;

            //--[ Localize Text ]----------
            if (mInitializeAlignment)
            {
                mInitializeAlignment = false;

                mAlignment_LTR = mAlignment_RTL = mTarget.alignment;

                if (LocalizationManager.IsRight2Left && mAlignment_RTL == TextAlignment.Right)
                    mAlignment_LTR = TextAlignment.Left;
                if (!LocalizationManager.IsRight2Left && mAlignment_LTR == TextAlignment.Left)
                    mAlignment_RTL = TextAlignment.Right;

            }
            if (mainTranslation != null && mTarget.text != mainTranslation)
            {
                if (cmp.CorrectAlignmentForRTL && mTarget.alignment != TextAlignment.Center)
                    mTarget.alignment = (LocalizationManager.IsRight2Left ? mAlignment_RTL : mAlignment_LTR);

                mTarget.text = mainTranslation;
            }
        }
    }

    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif

    public class LocalizeTarget_UnityStd_TextMesh : LocalizeTarget<TextMesh>
    {
        static LocalizeTarget_UnityStd_TextMesh() { AutoRegister(); }
        [RuntimeInitializeOnLoadMethod]
        static void AutoRegister()
        {
            LocalizationManager.RegisterTarget(new LocalizeTarget_UnityStd_TextMesh());
        }
        public TextAlignment mAlignment_RTL = TextAlignment.Right;
        public TextAlignment mAlignment_LTR = TextAlignment.Left;
        public bool mAlignmentWasRTL;
        public bool mInitializeAlignment = true;

        public override string GetName() { return "TextMesh"; }
        public override bool CanUseSecondaryTerm() { return true; }
        public override bool AllowMainTermToBeRTL() { return true; }
        public override bool AllowSecondTermToBeRTL() { return false; }

        public override void GetFinalTerms ( Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
        {
            var mTarget = GetTarget(cmp);
            primaryTerm = mTarget.text;
            secondaryTerm = (string.IsNullOrEmpty(Secondary) && mTarget.font != null) ? mTarget.font.name : null;
        }

        public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
        {
            var mTarget = GetTarget(cmp);

            //--[ Localize Font Object ]----------
            Font newFont = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
            if (newFont != null && mTarget.font != newFont)
                mTarget.font = newFont;

            //--[ Localize Text ]----------
            if (mInitializeAlignment)
            {
                mInitializeAlignment = false;

                mAlignment_LTR = mAlignment_RTL = mTarget.alignment;

                if (LocalizationManager.IsRight2Left && mAlignment_RTL == TextAlignment.Right)
                    mAlignment_LTR = TextAlignment.Left;
                if (!LocalizationManager.IsRight2Left && mAlignment_LTR == TextAlignment.Left)
                    mAlignment_RTL = TextAlignment.Right;

            }
            if (mainTranslation != null && mTarget.text != mainTranslation)
            {
                if (cmp.CorrectAlignmentForRTL && mTarget.alignment != TextAlignment.Center)
                    mTarget.alignment = (LocalizationManager.IsRight2Left ? mAlignment_RTL : mAlignment_LTR);

                mTarget.text = mainTranslation;
            }
        }
    }
}