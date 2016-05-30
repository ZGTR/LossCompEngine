using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ArithmeticCoder
{
    
    public class Coder
    {
        #region private members

        #region private members used by the coder/decoder methods
                        
        private ushort code;  
        private ushort low;   
        private ushort high;  
        private ulong underflow_bits; 

        #endregion

        #region members used by the bit I/O methods        

        private System.IO.MemoryStream output_buffer; 

        private System.IO.MemoryStream input_buffer;  


        private byte current_output_byte;        

        private byte current_input_byte;        

        private byte output_mask;                
                                                 
                                                 
                                                 
        private long input_bytes_left;          
        private ushort input_bits_left;         
                                                
                                                

        #endregion

        #endregion

        #region struct - symbol coding chars and probabilties

      
        public struct symbol
        {
            #region public members

            public char c;
            public ushort low;
            public ushort high;

            #endregion

            #region constructor           

            public symbol(char ch, ushort lval, ushort hval)
            {
                c = ch;
                low = lval;
                high = hval;
            }

            #endregion

            #region properties

            public ushort scale
            {
                get
                {
                    return Coder.scale;
                }
            }

            #endregion
        }        
                         
        #endregion

        
        #region character lookup table

       
        private static ushort scale = 0;

       
        private static List<symbol> codes;        

        #endregion        

        #region constructor

        public Coder(List<symbol> cds)
        {            
            Coder.codes = new List<symbol>();
            foreach (symbol s in cds)
                Coder.codes.Add(s);            
        }

        #endregion

        #region properties

        public List<symbol> Characters
        {
            get
            {
                return Coder.codes;
            }
        }

        public ushort Scale
        {
            get
            {
                return Coder.scale;
            }
            set
            {
                Coder.scale = value;
            }
        }

        #endregion

        #region methods

        #region compress/inflate functions

       
        public System.IO.MemoryStream compress(string input)
        {
            int i;
            char c;
            symbol s = new symbol();
                        
            initialize_output_bitstream();
            initialize_arithmetic_encoder();
            for (i = 0;i < input.ToCharArray().Length;i++)
            {                
                c = input.ToCharArray()[i];                
                convert_int_to_symbol(c,ref s);
                encode_symbol(s);
                if (c == '\0')
                    break;
            }
            flush_arithmetic_encoder();
            flush_output_bitstream();
            return output_buffer;
        }
    
       
        public string expand(System.IO.MemoryStream compressed_data, int size)
        {                      
            char c;
            ushort count;
            string retval = "";

            symbol s = new symbol();
            
            initialize_arithmetic_decoder(ref compressed_data);            
            for (int i = 0; ;)
            {                
                count = get_current_count(s);
                c = convert_symbol_to_int(count, ref s);
                retval += c.ToString();
                if (++i == size)
                    break;                
                remove_symbol_from_stream(s);
                
            }
            return retval;
        }

        
        private void convert_int_to_symbol( char c, ref symbol s )
        {
            int i;
            i=0;
            for ( ; ; )
            {
                if ( c == codes[ i ].c )
                {
                    s.c = c;
                    s.low = codes[ i ].low;
                    s.high = codes[ i ].high;                    
                    return;
                }                        
                if ( i == (codes.Count - 1)  )
                    error_exit( "Trying to encode a char not in the table" );
                i++;    
            }                        
        }

        
        private char convert_symbol_to_int(ushort count,ref symbol s)
        {
            int i;

            i = 0;
            for (; ; )
            {
                if (count >= codes[i].low && count < codes[i].high)
                {
                    s.low = codes[i].low;
                    s.high = codes[i].high;                    
                    return (codes[i].c);
                }
                if (i == (codes.Count - 1))
                    error_exit("Failure to decode character");
                i++;
            }
        }

        
        private void error_exit(string message)
        {
            throw new Exception(message);
        }

        #endregion

        #region coder/decoder methods

       
        private void initialize_arithmetic_encoder()
        {
            low = 0;
            high = 0xffff;
            underflow_bits = 0;
        }

        
        private void encode_symbol( symbol s )
        {
            long range;
            /*
             * These three lines rescale high and low for the new symbol.
             */ 
            range = (long) ( high-low ) + 1;

            high = Convert.ToUInt16(low + ((range * s.high) / s.scale - 1));

            low = Convert.ToUInt16(low + ((range * s.low) / s.scale));
            /*
             * This loop turns out new bits until high and low are far enough
             * apart to have stabilized.
             */
            for ( ; ; )
            {
                /*
                 * If this test passes, it means that the MSDigits match, and can
                 * be sent to the output stream.
                 */
                if ( ( high & 0x8000 ) == ( low & 0x8000 ) )
                {
                    output_bit(high & 0x8000 );
                    while ( underflow_bits > 0 )
                    {
                        output_bit(~high & 0x8000 );
                        underflow_bits--;                
                    }                                        
                }
                /*
                 * If this test passes, the numbers are in danger of underflow, because
                 * the MSDigits don't match, and the 2nd digits are just one apart.
                 */ 
                else if ( Convert.ToBoolean( low & 0x4000 ) && !Convert.ToBoolean( high & 0x4000 ))
                {
                    underflow_bits += 1;
                    low &= 0x3fff;
                    high |= 0x4000;                        
                }
                else
                    return ;
                low <<= 1;
                high <<= 1;
                high |= 1;                                                                
            }
        }

        
        private void flush_arithmetic_encoder()
        {
            output_bit( low & 0x4000 );
            underflow_bits++;
            while ( underflow_bits-- > 0 )
                output_bit( ~low & 0x4000 );              
        }

        
        private ushort get_current_count( symbol s )
        {
            long range;
            ushort count;
            range = (long) ( high - low ) + 1;
            count = (ushort)((((long) ( code - low ) + 1 ) * s.scale-1 ) / range );
            return( count );
        }

     
        private void initialize_arithmetic_decoder(ref System.IO.MemoryStream stream)
        {
            int i;

            initialize_input_bitstream(ref stream);

            code = 0;
            for (i = 0; i < 16; i++)
            {
                code <<= 1;
                code += input_bit();
            }
            low = 0;
            high = 0xffff;
        }

       
        private void remove_symbol_from_stream( symbol s)
        {
            long range;

         
            range = (long)(high - low) + 1;
            high = Convert.ToUInt16(low + (ushort)((range * s.high) / s.scale - 1));
            low = Convert.ToUInt16(low + (ushort)((range * s.low) / s.scale));
           
            for (; ; )
            {
                /*
                 * If the MSDigits match, the bits will be shifted out.
                 */
                if ((high & 0x8000) == (low & 0x8000))
                {
                }
                /*
                 * Else, if underflow is threatining, shift out the 2nd MSDigit.
                 */
                else if ((low & 0x4000) == 0x4000 && (high & 0x4000) == 0)
                {
                    code ^= 0x4000;
                    low &= 0x3fff;
                    high |= 0x4000;
                }
                /*
                 * Otherwise, nothing can be shifted out, so I return.
                 */
                else
                    return;
                low <<= 1;
                high <<= 1;
                high |= 1;
                code <<= 1;
                code += input_bit();
            }
        }

        #endregion

        #region bit I/O methods

     
        private void initialize_output_bitstream()
        {            
            output_buffer = new MemoryStream();
            current_output_byte = 0;            
            output_mask = 0x80;
        }

       
        private void output_bit(int bit)
        {            
            if (Convert.ToBoolean(bit))
                current_output_byte |= output_mask;
            output_mask >>= 1;
            if (output_mask == 0)
            {
                output_mask = 0x80;                
                output_buffer.WriteByte(current_output_byte);                
                current_output_byte = 0;
            }
        }

       
        private void flush_output_bitstream()
        {            
            output_buffer.WriteByte(current_output_byte);                        
        }

  
        private void initialize_input_bitstream(ref System.IO.MemoryStream stream_to_decode)
        {            
            input_buffer = stream_to_decode;
            input_bits_left = 0;
            input_bytes_left = stream_to_decode.Length;            
            current_input_byte = 0;            
            stream_to_decode.Seek(0, SeekOrigin.Begin);            
        }

      
        private ushort input_bit()
        {            
            if (input_bits_left == 0)
            {
                if (input_bytes_left > 0)
                    current_input_byte = Convert.ToByte(input_buffer.ReadByte());
                else
                    return 0;                         
                input_bytes_left--;
                input_bits_left = 8;                
            }
            input_bits_left--;
            return Convert.ToUInt16((current_input_byte >> input_bits_left) & 1);
        }

        
        private ulong bit_output()
        {
            ulong total;

            total = Convert.ToUInt32(output_buffer.Position);
            total += underflow_bits / 8;
            return (total);
        }

        
        private long bit_input()
        {
            return (input_buffer.Position - input_bytes_left + 1);
        }

        #endregion

        #endregion
    }
}
