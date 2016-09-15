#include "curlresponse.h"

void curl_init_string(struct curl_string *s)
{
    s->len = 0;
    s->ptr = (char*)malloc(s->len + 1);

    if (s->ptr == NULL)
    {
        fprintf(stderr, "malloc() failed\n");
        
        exit(EXIT_FAILURE);
    }

    s->ptr[0] = '\0';
}

size_t curl_write_string(void *ptr, size_t size, size_t nmemb, struct curl_string *s)
{
    if (s->ptr == NULL)
    {
        fprintf(stderr, "received NULL pointer to curl string; cannot realloc() memory.\n");

        exit(EXIT_FAILURE);
    }

    size_t new_len = s->len + size * nmemb;
    
    s->ptr = (char*)realloc(s->ptr, new_len + 1);
    
    if (s->ptr == NULL)
    {
        fprintf(stderr, "realloc() failed\n");
        
        exit(EXIT_FAILURE);
    }
    
    memcpy(s->ptr+s->len, ptr, size*nmemb);
    
    s->ptr[new_len] = '\0';
    s->len = new_len;

    return size * nmemb;
}