using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf.parser;
using System.Collections;
using AdministracionInstrumentos;



namespace gic_ManipuladorPdf
{
    public class man_ArchivoPdf
    {

        List<PdfReader> memoriaReaderes = new List<PdfReader>();
        List<string> pathsArchivos = new List<string>();
        
        private void borradoArchivos(List<string> archivosAborrar)
        {

            foreach (string archivo in archivosAborrar)
            {
                if (File.Exists(archivo))
                {
                    File.Delete(archivo);
                }
            }

        }

        public int getNumberOfPdfPages(string fileName)
        {
            using (StreamReader sr = new StreamReader(File.OpenRead(fileName)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }

        //CAMBIO JOSE VASQUEZ FECHA: 23.NOV.2015
        //MODIFICACION: SE INSERTA LOGICA 'CAMPO TIPOARCHIVO' PARA EL MANEJO DE SOPORTES PDF GENERAL E INDIGENA
        public void generarPdf(string nombreArchivo, string tipoArchivo, out  string rutaArchivo)
        {

           // blGenerarArchivo EntradaArchivo = new blGenerarArchivo();
            gic_adminconfig adminConfig = new gic_adminconfig();
            List<gic_adminconfig> _Config = new List<gic_adminconfig>();

            //CAMBIO JOSE VASQUEZ FECHA: 23.NOV.2015
            //MODIFICACION: SE INSERTA LOGICA 'CAMPO TIPOARCHIVO' PARA EL MANEJO DE SOPORTES PDF GENERAL E INDIGENA
            if (tipoArchivo == "GENERAL")
                _Config = adminConfig.GetAdminConfiguracion("ImprimiblePDF");
            else if (tipoArchivo == "INDIGENA")
                _Config = adminConfig.GetAdminConfiguracion("ImprimibleINDIGENAPDF"); ;
            //FIN CAMBIO JOSE VASQUEZ FECHA: 23.NOV.2015

            string path = _Config.First().ADMINCFG_VALUE;
            _Config = new List<gic_adminconfig>();
            _Config = adminConfig.GetAdminConfiguracion("path.outPdf");
            string pathR = _Config.First().ADMINCFG_VALUE;
            //string path = "c://Instrumentoimprimible.pdf";
            string oldFile = path;
            string newFile = pathR + nombreArchivo + ".pdf";

            rutaArchivo = newFile;
            // open the reader
            PdfReader reader = new PdfReader(oldFile);
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);
            // open the writer
            FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            for (var i = 1; i <= reader.NumberOfPages; i++)
            {
                document.NewPage();
                // the pdf content
                PdfContentByte cb = writer.DirectContent;

                // select the font properties
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.SetColorFill(iTextSharp.text.BaseColor.BLACK);
                cb.SetFontAndSize(bf, 8);
                // write the text in the pdf content
                cb.BeginText();
                string text = nombreArchivo;
                // put the alignment and coordinates here
                cb.ShowTextAligned(5, text, 660, 580, 0);
                //cb.ShowTextAligned(10, text, 100, 550, 0);
                cb.EndText();

                // create the new page and add it to the pdf
                PdfImportedPage page = writer.GetImportedPage(reader, i);
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string currentPageText = PdfTextExtractor.GetTextFromPage(reader, reader.NumberOfPages, strategy);
                cb.AddTemplate(page, 0, 0);
            }
            // close the streams and voilá the file should be changed :)
            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();
            List<string> pdfs = new List<string>();
            pdfs.Add(path);
            pdfs.Add(newFile);
            // _concatAndAddContent(pdfs);


        }

        public void ReadAlterPDFformDataInSelectedPages(out  string rutaArchivo)
        {
            string cadena = "";
            generarPdf("ENTRO", "ImprimiblePDF", out cadena);
            string path = cadena;
            string oldFile = path;
            PdfReader reader = new PdfReader(oldFile);
            string newFile = "c://Formato Comprobante PDF.pdf";
            rutaArchivo = newFile;
            AcroFields form = reader.AcroFields;
            //reader.SelectPages("1-2"); //Work with only page# 1 & 2
            using (PdfStamper stamper = new PdfStamper(reader, new FileStream(newFile, FileMode.Create)))
            {
                //AcroFields form = stamper.AcroFields;
                var fieldKeys = form.Fields.Keys;
                foreach (string fieldKey in fieldKeys)
                {
                    //Replace Address Form field with my custom data
                    if (fieldKey.Contains("#CODIGO"))
                    {
                        form.SetField(fieldKey, "CODIGOENTRO");
                    }
                }
                //The below will make sure the fields are not editable in
                //the output PDF.
                stamper.FormFlattening = true;
            }
        }

        public void FillAcroFields(string sourcePDFPath, string outputPDFPath, Hashtable values)
        {
            PdfReader pdfReader = new PdfReader(sourcePDFPath);
            PdfStamper stamper = new PdfStamper(pdfReader, new FileStream(outputPDFPath, FileMode.Create));
            AcroFields fields = stamper.AcroFields;
            AcroFields pdfFormFields = pdfReader.AcroFields;

            foreach (string key in values.Keys)
            {
                fields.SetField(key, values[key].ToString());
            }
            stamper.FormFlattening = true;
            stamper.Close();
        }
        public Document concatenarArchivosPdf(List<string> pdf, Stream os)
        {

            Document document = new Document();
            PdfCopy copy = new PdfCopy(document,os);
            document.Open();
            PdfImportedPage page;
            // this object **REQUIRED** to add content when concatenating PDFs
            PdfCopy.PageStamp stamp;
            // image watermark/background 
            /*iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(Request.MapPath("cat/kuujinbo2.gif"));
            pdfImage.SetAbsolutePosition(200, 400);*/
            // set transparency
            PdfGState state = new PdfGState();
            state.FillOpacity = 0.3F;
            state.StrokeOpacity = 0.3F;

            foreach (string p in pdf)
            {
                //  string pPath = Request.MapPath(string.Format("cat/{0}.pdf", p));
                string pPath = p;
                PdfReader reader = new PdfReader(new RandomAccessFileOrArray(pPath), null);
                int pages = reader.NumberOfPages;
                // loop over document pages
                for (int i = 0; i < pages; )
                {
                    page = copy.GetImportedPage(reader, ++i);
                    stamp = copy.CreatePageStamp(page);
                    PdfContentByte cb = stamp.GetUnderContent();
                    cb.SaveState();
                    cb.SetGState(state);
                    //cb.AddImage(pdfImage);
                    stamp.AlterContents();
                    copy.AddPage(page);
                }
                reader.Close();
            }
            document.Close();
            return document;
        }

        public byte[] crearPdfConTexto(string oldFile, string textoInsertar)
        {
            //Document doc = new Document(PageSize.LETTER, 50, 50, 50, 50);
            PdfReader reader = new PdfReader(oldFile);
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document doc = new Document(size);
            using (MemoryStream output = new MemoryStream())
            {
                PdfWriter wri = PdfWriter.GetInstance(doc, output);
                doc.Open();
                PdfContentByte cb = wri.DirectContent;
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.SetColorFill(iTextSharp.text.BaseColor.BLACK);
                cb.SetFontAndSize(bf, 8);
                // write the text in the pdf content
                string text = "";
                // write the text in the pdf content
                cb.BeginText();
                text = textoInsertar;
                // put the alignment and coordinates here
                cb.ShowTextAligned(5, text, 450, 380, 0);
                cb.EndText();
                PdfImportedPage page = wri.GetImportedPage(reader, 1);
                cb.AddTemplate(page, 0, 0);
                doc.Close();
                wri.Close();
                reader.Close();
                memoriaReaderes.Add(reader);
                return output.ToArray();
            }

        }


        private MemoryStream CreateMemorySteam(string oldFile)
        {
            //Document doc = new Document(PageSize.LETTER, 50, 50, 50, 50);
            PdfReader reader = new PdfReader(oldFile);
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document doc = new Document(size);
            using (MemoryStream output = new MemoryStream())
            {
                PdfWriter wri = PdfWriter.GetInstance(doc, output);
                doc.Open();
                PdfContentByte cb = wri.DirectContent;
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.SetColorFill(iTextSharp.text.BaseColor.DARK_GRAY);
                cb.SetFontAndSize(bf, 8);
                // write the text in the pdf content
                cb.BeginText();
                string text = "Some random blablablabla...";
                // put the alignment and coordinates here
                cb.ShowTextAligned(3, text, 120, 220, 0);
                cb.EndText();
                cb.BeginText();
                text = "Other random blabla...";
                // put the alignment and coordinates here
                cb.ShowTextAligned(2, text, 100, 200, 0);
                cb.EndText();
                PdfImportedPage page = wri.GetImportedPage(reader, 1);
                cb.AddTemplate(page, 0, 0);
                doc.Close();
                wri.Close();
                reader.Close();
                return output;
            }

        }

        /// <summary>
        /// Merges pdf files from a byte list
        /// </summary>
        /// <param name="files">list of files to merge</param>
        /// <returns>memory stream containing combined pdf</returns>
        public MemoryStream MergePdfForms(List<byte[]> files)
        {
            if (files.Count > 1)
            {
                string[] names;
                PdfStamper stamper;
                MemoryStream msTemp = null;
                PdfReader pdfTemplate = null;
                PdfReader pdfFile;
                Document doc;
                PdfWriter pCopy;
                MemoryStream msOutput = new MemoryStream();

                pdfFile = new PdfReader(files[0]);

                doc = new Document();
                pCopy = new PdfSmartCopy(doc, msOutput);
                pCopy.PdfVersion = PdfWriter.VERSION_1_7;

                doc.Open();

                for (int k = 0; k < files.Count; k++)
                {
                    for (int i = 1; i < pdfFile.NumberOfPages + 1; i++)
                    {
                        msTemp = new MemoryStream();
                        pdfTemplate = new PdfReader(files[k]);

                        stamper = new PdfStamper(pdfTemplate, msTemp);

                        names = new string[stamper.AcroFields.Fields.Keys.Count];
                        stamper.AcroFields.Fields.Keys.CopyTo(names, 0);
                        foreach (string name in names)
                        {
                            stamper.AcroFields.RenameField(name, name + "_file" + k.ToString());
                        }

                        stamper.Close();
                        pdfFile = new PdfReader(msTemp.ToArray());
                        ((PdfSmartCopy)pCopy).AddPage(pCopy.GetImportedPage(pdfFile, i));
                        pCopy.FreeReader(pdfFile);
                    }
                }

                pdfFile.Close();
                pCopy.Close();
                doc.Close();

                return msOutput;
            }
            else if (files.Count == 1)
            {
                return new MemoryStream(files[0]);
            }

            return null;
        }



        public MemoryStream MemoryStreamMerger(List<MemoryStream> streams)
        {

            MemoryStream OurFinalReturnedMemoryStream;
            using (OurFinalReturnedMemoryStream = new MemoryStream())
            {
                //Create our copy object
                PdfCopyFields copy = new PdfCopyFields(OurFinalReturnedMemoryStream);

                //Loop through each MemoryStream
                foreach (MemoryStream ms in streams)
                {
                    //Reset the position back to zero
                    ms.Position = 0;
                    //Add it to the copy object
                    copy.AddDocument(new PdfReader(ms));
                    //Clean up
                    ms.Dispose();
                }
                //Close the copy object
                copy.Close();

                //Get the raw bytes to save to disk
                //bytes = finalStream.ToArray();
            }
            return new MemoryStream(OurFinalReturnedMemoryStream.ToArray());

        }

        public void generarPdfSoporte(string nombreArchivo, out  string rutaArchivo)
        {

            gic_adminconfig adminConfig = new gic_adminconfig();
            List<gic_adminconfig> _Config = new List<gic_adminconfig>();
            _Config = adminConfig.GetAdminConfiguracion("ColillaPDF");

            string path = _Config.First().ADMINCFG_VALUE;
            _Config = new List<gic_adminconfig>();
            _Config = adminConfig.GetAdminConfiguracion("path.outPdf");
            string pathR = _Config.First().ADMINCFG_VALUE;
            //string path = "c://Instrumentoimprimible.pdf";
            string oldFile = path;
            string newFile = pathR + nombreArchivo + ".pdf";

            rutaArchivo = newFile;
            // open the reader
            PdfReader reader = new PdfReader(oldFile);
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);
            // open the writer
            FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            for (var i = 1; i <= reader.NumberOfPages; i++)
            {
                document.NewPage();
                // the pdf content
                PdfContentByte cb = writer.DirectContent;

                // select the font properties
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.SetColorFill(iTextSharp.text.BaseColor.BLACK);
                cb.SetFontAndSize(bf, 8);
                // write the text in the pdf content
                cb.BeginText();
                string text = "";
                // put the alignment and coordinates here
                cb.ShowTextAligned(5, text, 660, 580, 0);
                //cb.ShowTextAligned(10, text, 100, 550, 0);
                cb.EndText();

                // create the new page and add it to the pdf
                PdfImportedPage page = writer.GetImportedPage(reader, i);
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string currentPageText = PdfTextExtractor.GetTextFromPage(reader, reader.NumberOfPages, strategy);
                cb.AddTemplate(page, 0, 0);
            }
            // close the streams and voilá the file should be changed :)
            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();
            List<string> pdfs = new List<string>();
            pdfs.Add(path);
            pdfs.Add(newFile);
            // _concatAndAddContent(pdfs);


        }

        public Boolean descargarConstanciaSAAHSinFirmar(string nombreArchivo, string codigohogar,  out string rutaArchivo)
        {

            gic_adminconfig adminConfig = new gic_adminconfig();
            List<gic_adminconfig> _Config = new List<gic_adminconfig>();
            _Config = adminConfig.GetAdminConfiguracion("path.constanciasSAAH");

            string path = _Config.First().ADMINCFG_VALUE;
            _Config = new List<gic_adminconfig>();
            _Config = adminConfig.GetAdminConfiguracion("path.constanciasSAAH");
            string pathR = _Config.First().ADMINCFG_VALUE;            
            string oldFile = path+ "\\ConstanciaSAAH_"+ codigohogar +".pdf";
            string newFile = pathR + "ConstanciaSAAH_" + codigohogar + nombreArchivo + ".pdf";

            rutaArchivo = newFile;
            // open the reader
            if (File.Exists(oldFile))
            {
                PdfReader reader = new PdfReader(oldFile);
                Rectangle size = reader.GetPageSizeWithRotation(1);
                Document document = new Document(size);
                // open the writer
                FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                for (var i = 1; i <= reader.NumberOfPages; i++)
                {
                    document.NewPage();
                    // the pdf content
                    PdfContentByte cb = writer.DirectContent;

                    // select the font properties
                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetColorFill(iTextSharp.text.BaseColor.BLACK);
                    cb.SetFontAndSize(bf, 8);
                    // write the text in the pdf content
                    cb.BeginText();
                    string text = "";

                    cb.ShowTextAligned(5, text, 660, 580, 0);

                    cb.EndText();


                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentPageText = PdfTextExtractor.GetTextFromPage(reader, reader.NumberOfPages, strategy);
                    cb.AddTemplate(page, 0, 0);
                }

                document.Close();
                fs.Close();
                writer.Close();
                reader.Close();
                List<string> pdfs = new List<string>();
                pdfs.Add(path);
                pdfs.Add(newFile);

                return true;
            }
            else {
                return false;
            }

        }

    }
}
