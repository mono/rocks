MCS = gmcs
MCS_FLAGS = 
MONODOCER = monodocer
srcdir=.
PACKAGE = mono-rocks
VERSION = 0.1.0

Mono.Rocks.dll: Mono.Rocks.dll.sources $(shell cat Mono.Rocks.dll.sources)
	$(MCS) -debug+ -t:library -r:System.Core -out:$@ $(MCS_FLAGS) @$@.sources

all: Mono.Rocks.dll

include doc/Makefile.include

Mono.Rocks/Tuples.cs : mktuples Makefile
	./mktuples -n 4 > $@

Mono.Rocks/Lambdas.cs : mklambda Makefile
	./mklambda -n 4 > $@

Mono.Rocks/Curry.cs : mkcurry Makefile
	./mkcurry -n 4 > $@

check-gendarme:
	gendarme --html errors.html --ignore gendarme.ignore Mono.Rocks.dll

clean: doc-clean
	rm -f *.dll

run-test:

Mono.Rocks.Tests.dll: Mono.Rocks.Tests.dll.sources $(shell cat Mono.Rocks.Tests.dll.sources)
	$(MCS) -debug+ -r:Mono.Rocks.dll -r:System.Core -pkg:mono-nunit -t:library -out:$@ $(MCS_FLAGS) @$@.sources

run-test: Mono.Rocks.Tests.dll
	nunit-console2 Mono.Rocks.Tests.dll
