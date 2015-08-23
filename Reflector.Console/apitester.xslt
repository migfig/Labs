<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
    <xsl:output method="xml" indent="yes"/>
    
    <xsl:template match="/type">
      <xsl:call-template name="renderSetup"/>
    </xsl:template>

  <xsl:template name="renderSetup" match="type">
    <xsl:variable name="urlPrefix" select="concat('/', attributes/attribute[@type='System.Web.Http.RoutePrefixAttribute']/properties/property[@name='Prefix']/@value, '/')"/>
    <apiConfiguration>
      <setup baseAddress="http://localhost:60264" commandLine="curl.exe">
        <xsl:attribute name="source">
          <xsl:value-of select="@source"/>
        </xsl:attribute>

        <header name="Content-Type" value="application/json" />
        <buildHeader name="Authorization">
          <workflow>
            <task name="Authenticate" pattern="auth/authenticate">
              <inValue name="User" value="me"/>
              <inValue name="Password" value="pwd"/>
              <outValue name="code"/>
            </task>
            <task name="GetToken" pattern="auth/token">
              <inValue name="code" valueFromTask="Authenticate"/>
              <outValue name="token"/>
            </task>
          </workflow>
        </buildHeader>

        <xsl:for-each select="methods/method">
          <xsl:call-template name="renderMethod">
            <xsl:with-param name="urlPrefix" select="$urlPrefix"/>
          </xsl:call-template>
        </xsl:for-each>
      </setup>
    </apiConfiguration>
  </xsl:template>

  <xsl:template name="renderMethod" match="method">
    <xsl:param name="urlPrefix"/>
    <method>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>

      <xsl:attribute name="httpMethod">
        <xsl:if test="count(attributes/attribute[@type='System.Web.Http.HttpGetAttribute']/properties)">
          <xsl:value-of select="GET"/>
        </xsl:if>
      </xsl:attribute>

      <xsl:attribute name="url">
        <xsl:value-of select="concat($urlPrefix, attributes/attribute[@type='System.Web.Http.RouteAttribute']/properties/property[@name='Template']/@value)"/>
      </xsl:attribute>

      <xsl:attribute name="type">
        <xsl:value-of select="@type"/>
      </xsl:attribute>

      <xsl:for-each select="parameters/parameter">
        <xsl:call-template name="renderParameter"/>
      </xsl:for-each>
    </method>
  </xsl:template>

  <xsl:template name="renderProperty" match="property">
    <property>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>

      <xsl:attribute name="type">
        <xsl:value-of select="@type"/>
      </xsl:attribute>
    </property>
  </xsl:template>

  
  <xsl:template name="renderParameter" match="parameter">
    <parameter>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>

      <xsl:attribute name="type">
        <xsl:value-of select="@type"/>
      </xsl:attribute>
    </parameter>
  </xsl:template>
</xsl:stylesheet>
