using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace myTodoList.Service;

public static class UserTokenService
{
    private static SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));
    private static string issuer = "https://myToDoListProject.com";
    public static SecurityToken GetToken(List<Claim> claims) =>
      new JwtSecurityToken(
        issuer,
        issuer,
        claims,
        expires: DateTime.Now.AddDays(30.0),
        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
      );

   public static TokenValidationParameters GetTokenValidationParameters() =>
      new TokenValidationParameters
      {
        ValidIssuer = issuer,
        ValidAudience = issuer,
        IssuerSigningKey = key,
        ClockSkew = TimeSpan.Zero // remove delay of token when expire
      };

      public static string WriteToken(SecurityToken token) =>
        new JwtSecurityTokenHandler().WriteToken(token);
    }
