<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="text" standalone="yes"/>

  <xsl:template match="Slide">
    #<xsl:value-of select="@Title"/>#
    <xsl:apply-templates select="RichTextBlock"/>
    <xsl:apply-templates select="Component"/>
  </xsl:template>

  <xsl:template match="RichTextBlock">
    <xsl:apply-templates select="Paragraph"/>
  </xsl:template>

  <xsl:template match="Paragraph">
    <xsl:apply-templates select="Run"/>
    <xsl:apply-templates select="Bold"/>
  </xsl:template>

  <xsl:template match="Bold">
      *<xsl:value-of select="."/>*
  </xsl:template>

  <xsl:template match="Italics">
    **<xsl:value-of select="."/>**
  </xsl:template>

  <xsl:template match="Run">
    <xsl:value-of select="."/>
  </xsl:template>

  <xsl:template match="Component">
    <xsl:apply-templates select="Code"/>
  </xsl:template>

  <xsl:template match="LineBreak">
    <xsl:value-of select="string('#0x10#0x13')"/>      
  </xsl:template>

  <xsl:template match="Code">
    ```<xsl:value-of select="@Language"/>
        <xsl:value-of select="text()"/>
    ```
  </xsl:template>
</xsl:stylesheet>
