#ifndef __CURL_RESPONSE_H
#define __CURL_RESPONSE_H

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <curl/curl.h>

struct curl_string
{
    char* ptr;
    size_t len;
};

void curl_init_string(struct curl_string *s);

size_t curl_write_string(void *ptr, size_t size, size_t nmemb, struct curl_string *s);

#endif /* __CURL_RESPONSE_H */