MCS = gmcs
MCS_FLAGS = -langversion:linq

Mono.Rocks.dll: Mono.Rocks.dll.sources $(shell cat Mono.Rocks.dll.sources)
	$(MCS) -debug+ -t:library -r:System.Core -out:Mono.Rocks.dll $(MCS_FLAGS) @Mono.Rocks.dll.sources

all: Mono.Rocks.dll

Mono.Rocks/Tuples.cs : mktuples Makefile
	./mktuples -n 4 > $@

clean:
	rm -f *.dll

run-test:

Mono.Rocks.Tests.dll: Mono.Rocks.Tests.dll.sources $(shell cat Mono.Rocks.Tests.dll.sources)
	$(MCS) -debug+ -r:Mono.Rocks.dll -r:System.Core -pkg:mono-nunit -t:library -out:Mono.Rocks.Tests.dll $(MCS_FLAGS) @Mono.Rocks.Tests.dll.sources

run-test: Mono.Rocks.Tests.dll
	nunit-console2 Mono.Rocks.Tests.dll
