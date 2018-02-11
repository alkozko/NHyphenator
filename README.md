# NHyphenator

C# implementation of Frank Liang's hyphenation algorithm (also known as Knuth-Liang algorithm).
Read more about algorithm on http://en.wikipedia.org/wiki/Hyphenation_algorithm

This implementation contains original TEX hyphenation patterns (see http://tug.org/tex-hyphen/) for British and American English, and Russian language 

## NuGet

https://www.nuget.org/packages/NHyphenator/

## Example

### Simple usage example:

```c#
var loader = new ResourceHyphenatePatternsLoader(HyphenatePatternsLanguage.Russian);
Hypenator hypenator = new Hypenator(loader, "-");
var result = hypenator.HyphenateText(text);
```

### Adding new languages

This library contains build-in patterns for English and Russian languages (stored in .resx file)

You can add (or update) language patterns through using FilePatternsLoader and load patterns from files

```csharp
var loader = new new FilePatternsLoader($"{patterns_path}", $"{exceptions_path}");
```

Also you can create own implementation of `IHyphenatePatternsLoader` interface

You can find patterns [here](https://github.com/hyphenation/tex-hyphen/tree/master/hyph-utf8/tex/generic/hyph-utf8/patterns/txt):
`.pat.txt` files contain patterns, `.hyp.txt` files contain exceptions

## Licence

Source code are distributed under MIT licence. 
Hyphenation patterns are distributed under LaTeX Project Public License.

## Russian descripton

Подробнее о библиотеке можно прочесть (на русском) в моем блоге http://alkozko.ru/Blog/Post/liang-hyphenation-algorithm-on-c-sharp
