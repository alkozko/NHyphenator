NHyphenator
===========
C# implementation of Frank Liang's hyphenation algorithm (also known as Knuth-Liang algorithm) 
Read more about algorithm on http://en.wikipedia.org/wiki/Hyphenation_algorithm

This implementation contains original TEX hyphenation patterns (see http://tug.org/tex-hyphen/) for British and American English, and Russian language 

NuGet
===============
https://www.nuget.org/packages/NHyphenator/

Example
===============

Simple usage example:
```c#
Hypenator hypenator = new Hypenator(HypenatePatternsLanguage.EnglishUs, "-");
var result = hypenator.HyphenateText(text);
```

Licence
===============
Source code are distributed under MIT licence. 
Hyphenation patterns are distributed under LaTeX Project Public License.




Подробнее о библиотеке можно прочесть (на русском) в моем блоге http://alkozko.ru/Blog/Post/liang-hyphenation-algorithm-on-c-sharp
