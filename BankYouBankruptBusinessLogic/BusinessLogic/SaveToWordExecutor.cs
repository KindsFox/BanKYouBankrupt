﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using BankYouBankruptBusinessLogic.HelperModels;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BusinessLogic
{
    class SaveToWordExecutor
    {
        public static void CreateDoc(ListOperationInfoExecutor info)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument
                .Create(info.FileName, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body docBody = mainPart.Document.AppendChild(new Body());
                docBody.AppendChild(CreateParagraph(new WordParagraph
                {
                    Texts = new List<(string, WordTextProperties)> {
                        (info.Title, new WordTextProperties
                        {
                            Bold = true,
                            Size = "24",
                        })},
                    TextProperties = new WordTextProperties
                    {
                        Size = "24",
                        JustificationValues = JustificationValues.Center
                    }
                }));
                foreach (var cards in info.Cards)
                {
                    foreach (var operation in cards.Value.Operation)
                        docBody.AppendChild(CreateParagraph(new WordParagraph
                        {
                            Texts = new List<(string, WordTextProperties)> {
                            (cards.Value.CardsNumder + " - ",
                            new WordTextProperties
                            {
                                Bold = true,
                                Size = "24",
                            }),
                            (operation.Value.Item1 + " | ",
                            new WordTextProperties
                            {
                                Bold = false,
                                Size = "24",
                            }),
                            (operation.Value.Item2.ToString() + " | ",
                            new WordTextProperties
                            {
                                Bold = false,
                                Size = "24",
                            }),(operation.Value.Item3.ToString(),
                            new WordTextProperties
                            {
                                Bold = false,
                                Size = "24",
                            })
                        },
                            TextProperties = new WordTextProperties
                            {
                                Size = "24",
                                JustificationValues = JustificationValues.Both
                            }
                        }));
                }
                docBody.AppendChild(CreateSectionProperties());
                wordDocument.MainDocumentPart.Document.Save();
            }
        }

        private static SectionProperties CreateSectionProperties()
        {
            SectionProperties properties = new SectionProperties();
            PageSize pageSize = new PageSize
            {
                Orient = PageOrientationValues.Portrait
            };
            properties.AppendChild(pageSize);
            return properties;
        }

        private static Paragraph CreateParagraph(WordParagraph paragraph)
        {
            if (paragraph != null)
            {
                Paragraph docParagraph = new Paragraph();

                docParagraph.AppendChild(CreateParagraphProperties(paragraph.TextProperties));
                foreach (var run in paragraph.Texts)
                {
                    Run docRun = new Run();
                    RunProperties properties = new RunProperties();
                    properties.AppendChild(new FontSize { Val = run.Item2.Size });
                    if (run.Item2.Bold)
                    {
                        properties.AppendChild(new Bold());
                    }
                    docRun.AppendChild(properties);
                    docRun.AppendChild(new Text
                    {
                        Text = run.Item1,
                        Space = SpaceProcessingModeValues.Preserve
                    });
                    docParagraph.AppendChild(docRun);
                }
                return docParagraph;
            }
            return null;
        }

        private static ParagraphProperties CreateParagraphProperties(WordTextProperties paragraphProperties)
        {
            if (paragraphProperties != null)
            {
                ParagraphProperties properties = new ParagraphProperties();
                properties.AppendChild(new Justification()
                {
                    Val = paragraphProperties.JustificationValues
                });
                properties.AppendChild(new SpacingBetweenLines
                {
                    LineRule = LineSpacingRuleValues.Auto
                });
                properties.AppendChild(new Indentation());
                ParagraphMarkRunProperties paragraphMarkRunProperties = new ParagraphMarkRunProperties();
                if (!string.IsNullOrEmpty(paragraphProperties.Size))
                {
                    paragraphMarkRunProperties.AppendChild(new FontSize
                    {
                        Val = paragraphProperties.Size
                    });
                }
                properties.AppendChild(paragraphMarkRunProperties);
                return properties;
            }
            return null;
        }
    }
}

