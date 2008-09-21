MCS = gmcs
# warning CS1591: Missing XML comment for publicly visible type or member...
MCS_FLAGS = -nowarn:1591
MONODOCER = monodocer
srcdir=.
PACKAGE = mono-rocks
VERSION = 0.1.0

Mono.Rocks.dll: Mono.Rocks.dll.sources $(shell cat Mono.Rocks.dll.sources)
	$(MCS) -doc:doc/mono-rocks.xml -debug+ -t:library -r:System.Core -out:$@ $(MCS_FLAGS) @$@.sources

all: Mono.Rocks.dll

include doc/Makefile.include

mkdelegates mkeithers mklambda mktuples : Generator.pm

Mono.Rocks/Eithers.cs : mkeithers Makefile
	./mkeithers -n 4 > $@

Mono.Rocks/Tuples.cs : mktuples Makefile
	./mktuples -n 4 > $@

Mono.Rocks/Lambdas.cs : mklambda Makefile
	./mklambda -n 4 > $@

Mono.Rocks/Delegates.cs : mkdelegates Makefile
	./mkdelegates -n 4 > $@

check-gendarme:
	gendarme --html errors.html --ignore gendarme.ignore Mono.Rocks.dll

clean: doc-clean
	rm -f *.dll

run-test:

Mono.Rocks.Tests.dll: Mono.Rocks.Tests.dll.sources $(shell cat Mono.Rocks.Tests.dll.sources) Mono.Rocks.dll
	$(MCS) -debug+ -r:Mono.Rocks.dll -r:System.Core -pkg:mono-nunit -t:library -out:$@ $(MCS_FLAGS) @$@.sources

run-test: Mono.Rocks.Tests.dll
	nunit-console2 Mono.Rocks.Tests.dll
