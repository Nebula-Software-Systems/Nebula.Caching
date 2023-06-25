As you might already know, there's a limit on the amount of memory available on the machine that is running your .NET applicaton, so you must be very careful when taking the decision of taking a portion of that memory for caching purposes.

Because of the constraint presented above, our in-memory solution uses a compression algorithm when storing your cached data. Of course, this comes with the cost of running the compression algorithm (and the decompression one when retrieving the cached data), but that's a worthwhile cost to pay.
