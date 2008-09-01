# Fixup //member/@name (because gmcs output doesn't match CSC convention)

# Type.ICollection.Method``(args)
/<member name=.*\.\(IList\|ICollection\|IEnumerable\)\..*``1/ {
	s/\(<member name=".*\).\(IList\|ICollection\|IEnumerable\)\.\(.*\)``1/\1.System#Collections#Generic#\2{System#Object}#\3/
	p
	d
}

# Type.ICollection`1.Property
/<member name=.*\.\(IList\|ICollection\)`1\./ {
	s/\(<member name=".*\)\.\(IList\|ICollection\)`1\.\(.*\)/\1.System#Collections#Generic#\2{System#Object}#\3/
	p
	d
}

# Type.ICollection.Member
/<member name=.*\.\(IList\|ICollection\|IEnumerable\)\./ {
	s/\(<member name=".*\).\(IList\|ICollection\|IEnumerable\).\(.*\)/\1.System#Collections#\2#\3/
	p
	d
}

