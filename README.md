# MZCachetastic (for .net)

A very simple to implement caching mechanism for .net.

#NuGet

A NuGet package is TBA.

## IMPLEMENTATION:

    var result = Cachetastic.Fetch(key, () -> DoSomething());
    
## IS IT WORKING:

The first use of Cachetastic for the key will execute the Func<T> and store/return the result. Subsequent fetch calls to Cachetastic will match on the key (and optionally hashcode) and returned the cached value.

## CAVEATS:

Cachetastic may not return the cached values if:

* The cached value is old (default: 5 minutes).
* If the hashcode for the key is different (more on the hashcode usage later).

## REQUIREMENTS:

Requires .Net 4 (dependancy on ConcurrentDictionary)


## LICENSE:

(The MIT License)

Copyright (c) 2011 Ben Clark-Robinson, ben.clarkrobinson@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
'Software'), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
