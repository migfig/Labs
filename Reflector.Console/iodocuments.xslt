<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
    <xsl:output method="text" indent="yes"/>
    
    <xsl:template match="/type/methods">
      <xsl:call-template name="renderMethods"/>
    </xsl:template>

  <xsl:template name="renderMethods" match="methods">
    {
    "schemas": {
    <xsl:for-each select="method/parameters/parameter">
      <xsl:call-template name="renderParameter">
        <xsl:with-param name="islast" select="position() = last()"/>
      </xsl:call-template>
    </xsl:for-each>
    },
    "resources": {
    "Application-Methods": {
    "methods": {
    <xsl:for-each select="method">
          <xsl:call-template name="renderMethod">
            <xsl:with-param name="islast" select="position() = last()"/>
          </xsl:call-template>
        </xsl:for-each>
      }
    }
  }
  }
  </xsl:template>

  <xsl:template name="renderMethod" match="method">
    <xsl:param name="islast"/>
    "<xsl:value-of select="@name"/>": {
      "description": "<xsl:value-of select="@name"/>",      
      <xsl:for-each select="parameters">
        <xsl:call-template name="renderParameters"/>
      </xsl:for-each>
    }<xsl:if test="not($islast)">,</xsl:if>
  </xsl:template>

  <xsl:template name="renderProperty" match="property">
    "<xsl:value-of select="@name"/>": {
      "type": "<xsl:value-of select="@type"/>",
      "required": true,
      "location": "body",
      "description": "<xsl:value-of select="@name"/>"
    }
  </xsl:template>

  <xsl:template name="renderParameters" match="parameters">
    "parameters": {
      <xsl:for-each select="parameter">
        <xsl:call-template name="renderParameter">
          <xsl:with-param name="islast" select="position() = last()"/>
        </xsl:call-template>
      </xsl:for-each>
    }
  </xsl:template>
  
  <xsl:template name="renderParameter" match="parameter">
    <xsl:param name="islast"/>
    "<xsl:value-of select="@name"/>": {
    "type": "<xsl:value-of select="@type"/>",
    "required": true,
    "location": "query",
    "description": "<xsl:value-of select="@name"/>"
    }<xsl:if test="not($islast)">,</xsl:if>
  </xsl:template>
</xsl:stylesheet>
