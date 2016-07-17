### Embedding ChakraCore on Linux and macOS

Clone ChakraCore Repository
```
git clone https://github.com/Microsoft/ChakraCore
```

Compile ChakraCore
```
cd ChakraCore
./build.sh --static --debug
```

*For macOS add `--icu=[path to icu]`*

Copy the files from `CrossPlatform` folder into `ChakraCore/CrossPlatform`

```
cd CrossPlatform
make BUILD_TYPE=Debug
```

*For macOS add `PLATFORM=darwin`*

Run!
```
./sample.o
```
