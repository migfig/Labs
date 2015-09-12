<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                xmlns:utils="urn:schemas-reflector-com:xslt">
  <xsl:output method="text" indent="yes"/>

  <xsl:template match="/type">
    <xsl:call-template name="renderSwagger"/>
  </xsl:template>

  <xsl:template name="renderSwagger" match="type">
    #Swagger definition being built
  </xsl:template>
</xsl:stylesheet>
