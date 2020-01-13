using System;

namespace NHyphenator.Loaders
{
    public class ResourceHyphenatePatternsLoader : IHyphenatePatternsLoader
    {
        private readonly string _patterns;
        private readonly string _exceptions;

        public ResourceHyphenatePatternsLoader(HyphenatePatternsLanguage language)
        {
            switch (language)
            {
                case HyphenatePatternsLanguage.EnglishUs:
                    _patterns = Patterns.hyph_en_us_pat;
                    _exceptions = Patterns.hyph_en_us_hyp;
                    break;
                case HyphenatePatternsLanguage.EnglishBritish:
                    _patterns = Patterns.hyph_en_gb_pat;
                    _exceptions = Patterns.hyph_en_us_hyp;
                    break;
                case HyphenatePatternsLanguage.Russian:
                    _patterns = Patterns.hyph_ru_pat;
                    _exceptions = Patterns.hyph_ru_hyp;
                    break;
                case HyphenatePatternsLanguage.German1901:
                    _patterns = Patterns.hyph_de_1901_pat;
                    _exceptions = Patterns.hyph_de_1901_hyp;
                    break;
                case HyphenatePatternsLanguage.German1996:
                    _patterns = Patterns.hyph_de_1996_pat;
                    _exceptions = Patterns.hyph_de_1996_hyp;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("language");
            }

        }


        public string LoadExceptions() => _exceptions;

        public string LoadPatterns() => _patterns;
    }
}