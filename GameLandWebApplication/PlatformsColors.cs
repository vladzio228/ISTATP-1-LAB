using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLandWebApplication
{
    public static class PlatformsColors
    {
        public static string ToColor(string platform)
        {
            switch(platform){
                case "PC":              return "#296fda";                  
                case "PS3":             return "#23527c";                 
                case "PS4":             return "#23527c";                   
                case "PS5":             return "#23527c";                    
                case "Xbox":            return "#0ac70a";                  
                case "Xbox 360":        return "#0ac70a";                   
                case "Xbox One":        return "#0ac70a";                   
                case "Xbox Series X":   return "#0ac70a";                
                case "Xbox Series S":   return "#0ac70a";                 
                case "Nintendo Switch": return "#ed1c24";                  
                case "iOS":             return "#D201EB";                
                case "Android":         return "#b0ef42";                  
                default:                return "#296fda";
                    
            }
        }
        public static string TopColors(int number)
        {
            return number == 1 ? "#E0E900" :
                   number == 2 ? "#A6C3C4" :
                   number == 3 ? "#E0A12B" :
                                 "#E5DFDF" ;
        }
    }
    
}
