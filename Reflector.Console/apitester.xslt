<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                xmlns:utils="urn:schemas-reflector-com:xslt">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/type">
    <xsl:call-template name="renderSetup"/>
  </xsl:template>

  <xsl:template name="renderSetup" match="type">
    <xsl:variable name="urlPrefix" select="concat('/', attributes/attribute[@type='System.Web.Http.RoutePrefixAttribute']/properties/property[@name='Prefix']/@value, '/')"/>
    <apiConfiguration>
      <setup commandLine="C:\Program Files (x86)\Git\bin\curl.exe">
        <xsl:attribute name="source">
          <xsl:value-of select="@source"/>
        </xsl:attribute>

        <host name="localhost">http://localhost:60264</host>
        <host name="remotehost">http://remote:60264</host>
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
      </setup>
      <xsl:for-each select="methods/method">
        <xsl:call-template name="renderMethod">
          <xsl:with-param name="urlPrefix" select="$urlPrefix"/>
        </xsl:call-template>
      </xsl:for-each>
    </apiConfiguration>
  </xsl:template>

  <xsl:template name="renderMethod" match="method">
    <xsl:param name="urlPrefix"/>
    <method>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>

      <xsl:attribute name="httpMethod">
        <xsl:value-of select="utils:HttpMethod(attributes/attribute/@type='System.Web.Http.HttpGetAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpPostAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpPutAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpPatchAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpDeleteAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpOptionsAttribute')"/>
      </xsl:attribute>

      <xsl:attribute name="url">
        <xsl:value-of select="concat($urlPrefix, attributes/attribute[@type='System.Web.Http.RouteAttribute']/properties/property[@name='Template']/@value)"/>
      </xsl:attribute>

      <xsl:attribute name="type">
        <xsl:value-of select="attributes/attribute[@type='System.Web.Http.Description.ResponseTypeAttribute']/properties/property[@name='ResponseType']/@value"/>
      </xsl:attribute>

      <xsl:attribute name="description">
        <xsl:value-of select="utils:CapitalizeWords(@name)"/>
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

  <xsl:template name="renderPropertyAsJson" match="property">
    <xsl:param name="isLast"/>
    "<xsl:value-of select="@name"/>": <xsl:choose>
      <xsl:when test="@type = 'System.String'">
        <xsl:value-of select="concat('&quot;',@name, @defaultValue,' {0}&quot;')" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="@defaultValue"/>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:if test="not($isLast)">
      <xsl:value-of select="string(',')"/>
    </xsl:if>
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

      <xsl:if test ="attributes/attribute/@type='System.Web.Http.FromBodyAttribute'">
        <jsonObject>
          &lt;![CDATA[
          {<xsl:for-each select="properties/property">
            <xsl:call-template name="renderPropertyAsJson">
              <xsl:with-param name="isLast" select="position() = last()"/>
            </xsl:call-template>
          </xsl:for-each>
          }]]&gt;
        </jsonObject>
      </xsl:if>
    </parameter>
  </xsl:template>
</xsl:stylesheet>
