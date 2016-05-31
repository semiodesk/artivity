<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:param name="title">Title</xsl:param>

<xsl:output method="html" 
encoding="UTF-8" indent="yes"/>

<xsl:template match="/">
  <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html&gt;</xsl:text>
  
  <html>
  <head>
  </head>
  <body>
  
  <h1><xsl:value-of select="$title"/></h1>
  
  <ul>
  
  <xsl:for-each select="changelog/channel[contains(@os, 'osx')]/notes[not(../notes/@id > @id)]/note">
    <li><xsl:value-of select="."/></li>
  </xsl:for-each>
  
  </ul>
  </body>
  </html>
  
  </xsl:template>

</xsl:stylesheet>
