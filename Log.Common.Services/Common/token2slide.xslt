<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" standalone="yes" cdata-section-elements="Code"/>

  <xsl:template match="tokens">
    <Slide>
      <xsl:attribute name="Title">
        <xsl:value-of select="token[@type='H1'][@value]"/>
      </xsl:attribute>
      <RichTextBlock FontSize="20" FontWeight="DemiBold" LineHeight="40">
        <xsl:apply-templates select="token"/>
      </RichTextBlock>
      <xsl:apply-templates select="code"/>
    </Slide>
  </xsl:template>

  <xsl:template match="token[@type='Bold']">
    <Paragraph>
      <Bold>
        <xsl:value-of select="@value"/>
      </Bold>
    </Paragraph>
  </xsl:template>

  <xsl:template match="token[@type='Italics']">
    <Paragraph>
      <Run>
        <xsl:attribute name="FontStyle">
          <xsl:value-of select="string('Italic')"/>
        </xsl:attribute>
        <xsl:value-of select="@value"/>
      </Run>
    </Paragraph>
  </xsl:template>

  <xsl:template match="token[@type='Regular']">
    <Paragraph>
      <xsl:value-of select="@value"/>
    </Paragraph>
  </xsl:template>

  <xsl:template match="token[@type='NewLine']">
    <Paragraph>
      <LineBreak />
    </Paragraph>
  </xsl:template>

  <xsl:template match="code">
    <Component>
      <xsl:attribute name="Language">
        <xsl:value-of select="@language"/>
      </xsl:attribute>
      <Code>
        <xsl:value-of select="text()"/>
      </Code>
    </Component>
  </xsl:template>
</xsl:stylesheet>
