#! /bin/sh

./configure
make
if [ ! -d "output" ]; then
  mkdir output
fi
cp src/.libs/libartivity.a output
