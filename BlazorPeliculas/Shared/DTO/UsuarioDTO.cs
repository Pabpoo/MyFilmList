﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTO
{
    public class UsuarioDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
		public List<string> Roles { get; set; }
	}
}
