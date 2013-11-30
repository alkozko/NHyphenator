NHyphenator
===========
C# implementation of Frank Liang's hyphenation algorithm (also known as Knuth-Liang algorithm) 
Read more about algorithm on http://en.wikipedia.org/wiki/Hyphenation_algorithm

This implementation contains original TEX hyphenation patterns (see http://tug.org/tex-hyphen/) for British and American English, and Russian language 


Simple usage example:
Hypenator hypenator = new Hypenator(HypenatePatternsLanguage.EnglishUs, "-");
var result = hypenator.HyphenateText(text);
