#!/usr/bin/perl
package Generator;

use strict;

my $num_system_funcs = 4;

sub new {
	my ($class, $handle) = @_;

	my $self = { 
		output    => $handle,
		indented  => 0,
		indent    => 0
	};
	bless $self, $class;
	return $self;
}

sub MaxSystemFuncs {
	return $num_system_funcs;
}

sub Action {
	my ($self, $n) = @_;

	if ($n <= $num_system_funcs) {
		$self->Write ("Action");
	} else {
		$self->Write ("RocksAction");
	}
}

sub Func {
	my ($self, $n) = @_;

	if ($n <= $num_system_funcs) {
		$self->Write ("Func");
	} else {
		$self->Write ("RocksFunc");
	}
}

sub Write {
	my ($self, @args) = @_;

	my $s = join "", @args;
	my @lines = split /\n/, $s, -1;

	my $endnl = $s =~ m/\n$/;

	my $h = $self->{output};

	if (length ($lines [0]) > 0) {
		$self->WriteIndent ();
	}
	print $h $lines [0];

	for (my $i = 1; $i < @lines; ++$i) {
		print $h "\n";
		if (length ($lines [$i]) > 0) {
			$self->{indented} = 0;
			$self->WriteIndent ();
		}
		print $h $lines [$i];
	}

	if ($endnl) {
		$self->{indented} = 0;
	}
	return $self;
}

sub WriteIndent {
	my ($self) = @_;

	if (!$self->{indented}) {
		my $h = $self->{output};
		print $h "\t" x $self->{indent};
		$self->{indented} = 1;
	}
	return $self;
}

sub WriteLicense {
	my ($self) = @_;

	my $license = <<EOF;
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
EOF
	return $self->Write ($license);
}

sub _write {
	my ($self, $v) = @_;

	if (!ref ($v)) {
		$self->Write ($v);
	} else {
		$v->($self);
	}
}

sub Block {
	my ($self, $body) = @_;

	$self->Write (" ") if ($self->{indented});
	$self->Write ("{\n");
	++$self->{indent};
	_write ($self, $body);
	--$self->{indent};
	return $self->Write ("}\n");
}

sub Namespace {
	my ($self, $namespace, @types) = @_;

	$self->Write ("namespace ", $namespace);
	return $self->Block (
		sub { foreach (@types) { $_->($self); } }
	);
}

sub Type {
	my ($self, $type, $body) = @_;

	_write ($self, $type);
	return $self->Block ($body);
}

sub Method {
	my ($self, $ret, $name, $args, $body) = @_;

	_write ($self, $ret);
	_write ($self, $name);
	$self->Write (" (");
	_write ($self, $args);
	$self->Write (")\n");
	return $self->Block ($body);
}

sub TypeParameter {
	my ($self, $max, $i) = @_;
	if ($max == 1) {
		$self->Write ("T");
	}
	else {
		$self->Write ("T$i");
	}
	return $self;
}

sub TypeParameterList {
	my ($self, $max, $start, $end) = @_;
	$start ||= 1;
	$end   ||= $max;

	$self->TypeParameter ($max, $start);
	for (my $i = $start + 1; $i <= $end; ++$i) {
		$self->Write (", ");
		$self->TypeParameter ($max, $i);
	}
	return $self;
}

sub Value {
	my ($self, $max, $i) = @_;
	if ($max == 1) {
		$self->Write ("value");
	}
	else {
		$self->Write ("value$i");
	}
	return $self;
}

sub ValueList {
	my ($self, $max, $start, $end) = @_;
	$start ||= 1;
	$end   ||= $max;

	return if $max == 0;

	$self->Value ($max, $start);
	for (my $i = $start + 1; $i <= $end; ++$i) {
		$self->Write (", ");
		$self->Value ($max, $i);
	}
	return $self;
}

sub MethodParameter {
	my ($self, $max, $i) = @_;

	if ($max == 1) {
		$self->Write ("T value");
	}
	else {
		$self->Write ("T$i value$i");
	}
	return $self;
}

sub MethodParameterList {
	my ($self, $max, $start, $end) = @_;
	$start ||= 1;
	$end   ||= $max;

	return if $max == 0;

	$self->MethodParameter ($max, $start);
	for (my $i = $start + 1; $i <= $end; ++$i) {
		$self->Write (", ");
		$self->MethodParameter ($max, $i);
	}
	return $self;
}

1;

