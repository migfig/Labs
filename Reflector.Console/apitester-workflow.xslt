<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                xmlns:utils="urn:schemas-reflector-com:xslt">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/type">
    <xsl:call-template name="renderWorkflow"/>
  </xsl:template>

  <xsl:template name="renderWorkflow" match="type">
    <workflow>
      <xsl:attribute name="name">
        <xsl:value-of select="utils:FileName(@source)"/>
      </xsl:attribute>

      <xsl:for-each select="methods/method">
        <xsl:call-template name="renderTask"/>
      </xsl:for-each>
    </workflow>
  </xsl:template>

  <xsl:template name="renderTask" match="method">
    <task>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>

      <xsl:for-each select="parameters/parameter">
        <xsl:call-template name="renderParameter"/>
      </xsl:for-each>

      <resultValue condition="And" propertyName="Id" operator="isEqualTo" value="1"/>
    </task>
  </xsl:template>

  <xsl:template name="renderParameter" match="parameter">
    <parameter>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>

      <xsl:attribute name="type">
        <xsl:value-of select="@type"/>
      </xsl:attribute>

      <xsl:attribute name="location">
        <xsl:value-of select="utils:Iif(attributes/attribute/@type='System.Web.Http.FromBodyAttribute', 'body', 'query')"/>
      </xsl:attribute>

      <xsl:if test ="not(attributes/attribute/@type='System.Web.Http.FromBodyAttribute')">
        <xsl:attribute name="defaultValue">
          <xsl:value-of select="@defaultValue"/>
        </xsl:attribute>
      </xsl:if>

      <xsl:if test ="attributes/attribute/@type='System.Web.Http.FromBodyAttribute'">
        <jsonObject>
          <xsl:value-of select="string('&lt;![CDATA[')" disable-output-escaping="yes"/>
          {<xsl:for-each select="properties/property">
            <xsl:call-template name="renderProperty">
              <xsl:with-param name="isLast" select="position() = last()"/>
            </xsl:call-template>
          </xsl:for-each>
          }
          <xsl:value-of select="string(']]&gt;')" disable-output-escaping="yes"/>
        </jsonObject>
      </xsl:if>
    </parameter>
  </xsl:template>

  <xsl:template name="renderProperty" match="property">
    <xsl:param name="isLast"/>

    <xsl:choose>
      <xsl:when test="@isArray = 'true'">
        <xsl:call-template name="renderPropertyAsJsonArray">
          <xsl:with-param name="isLast" select="$isLast"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="properties/property">
        <xsl:call-template name="renderPropertyAsJsonObject">
          <xsl:with-param name="isLast" select="$isLast"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="renderPropertyAsJson">
          <xsl:with-param name="isLast" select="$isLast"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="renderPropertyAsJson" match="property">
    <xsl:param name="isLast"/>
    "<xsl:value-of select="@name"/>": <xsl:choose>
      <xsl:when test="@type = 'System.String'">
        <xsl:value-of select="concat('&quot;',@name, @defaultValue,'&quot;')" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="@defaultValue"/>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:if test="not($isLast)">
      <xsl:value-of select="string(',')"/>
    </xsl:if>
  </xsl:template>

  <xsl:template name="renderPropertyAsJsonArray" match="property">
    <xsl:param name="isLast"/>
    "<xsl:value-of select="@name"/>":
    [
    {<xsl:for-each select="properties/property">
      <xsl:call-template name="renderProperty">
        <xsl:with-param name="isLast" select="position() = last()"/>
      </xsl:call-template>
    </xsl:for-each>
    }
    ]<xsl:if test="not($isLast)">
      <xsl:value-of select="string(',')"/>
    </xsl:if>
  </xsl:template>

  <xsl:template name="renderPropertyAsJsonObject" match="property">
    <xsl:param name="isLast"/>
    "<xsl:value-of select="@name"/>": {
    <xsl:for-each select="properties/property">
      <xsl:call-template name="renderProperty">
        <xsl:with-param name="isLast" select="position() = last()"/>
      </xsl:call-template>
    </xsl:for-each>
    }<xsl:if test="not($isLast)">
      <xsl:value-of select="string(',')"/>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
