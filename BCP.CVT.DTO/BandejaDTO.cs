﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class BandejaDTO: TreeElementDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class BandejaAprobacionDTO : TreeElementDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public int BandejaId { get; set; }
        public string MatriculaAprobador { get; set; }
        public string Correo { get; set; }
        public string Nombres { get; set; }
        public bool? FlagValidarMatricula { get; set; }

        //NotMapped
        public bool IsLastLevel => true;
    }
}
