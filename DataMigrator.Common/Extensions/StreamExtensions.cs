using System.Diagnostics;
using System.Text;

namespace DataMigrator.Common.Extensions
{
    /// <summary>
    /// https://github.com/NimaAra/Easy.Common/blob/master/Easy.Common/Extensions/StreamExtensions.cs
    /// https://nimaara.com/2018/03/20/counting-lines-of-a-text-file-in-csharp.html
    /// </summary>
    public static class StreamExtensions
    {
        private const char CR = '\r';
        private const char LF = '\n';
        private const char NULL = (char)0;

        /// <summary>
        /// Returns the number of lines in the given <paramref name="stream"/>.
        /// </summary>
        [DebuggerStepThrough]
        public static int CountLines(this Stream stream, Encoding? encoding = default)
        {
            int lineCount = 0;
            byte[] byteBuffer = new byte[1024 * 1024];
            char detectedEOL = NULL;
            char currentChar = NULL;
            int bytesRead;

            if (encoding is null || Equals(encoding, Encoding.ASCII) || Equals(encoding, Encoding.UTF8))
            {
                while ((bytesRead = stream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                {
                    for (int i = 0; i < bytesRead; i++)
                    {
                        currentChar = (char)byteBuffer[i];

                        if (detectedEOL != NULL)
                        {
                            if (currentChar == detectedEOL)
                            {
                                lineCount++;
                            }
                        }
                        else if (currentChar is LF or CR)
                        {
                            detectedEOL = currentChar;
                            lineCount++;
                        }
                    }
                }
            }
            else
            {
                char[] charBuffer = new char[byteBuffer.Length];

                while ((bytesRead = stream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                {
                    int charCount = encoding.GetChars(byteBuffer, 0, bytesRead, charBuffer, 0);

                    for (var i = 0; i < charCount; i++)
                    {
                        currentChar = charBuffer[i];

                        if (detectedEOL != NULL)
                        {
                            if (currentChar == detectedEOL)
                            {
                                lineCount++;
                            }
                        }
                        else if (currentChar is LF or CR)
                        {
                            detectedEOL = currentChar;
                            lineCount++;
                        }
                    }
                }
            }

            if (currentChar != LF && currentChar != CR && currentChar != NULL)
            {
                lineCount++;
            }

            return lineCount;
        }
    }
}