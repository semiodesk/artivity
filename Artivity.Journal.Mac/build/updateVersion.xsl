<!--  Invoke with xsltproc -param build-number 4 updateVersion.xsl Info.plist > Info.plist /-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="build-number">2</xsl:param>

  <xsl:template match="node() | @*">
    <xsl:copy>
      <xsl:apply-templates select="node() | @*" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="/plist/dict/string">
    <xsl:choose>
      <xsl:when test="preceding-sibling::key[1] = 'CFBundleVersion'">
        <string><xsl:value-of select="$build-number"/></string>
      </xsl:when>
      <xsl:otherwise>
        <string><xsl:value-of select="."/></string>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>
