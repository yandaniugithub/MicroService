// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer_ResourceOwnerPasswordCredentials
{
    public static class Config
    {
        //public static IEnumerable<IdentityResource> IdentityResources =>
        //    new IdentityResource[]
        //    { 
        //        new IdentityResources.OpenId()
        //    };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            { 
                new ApiScope("Yak.Ocelot.Api", "My API"),
                new ApiScope("Yak.Cap.RabbitMQ.SubscribeApi", "Product API"),
                new ApiScope("Yak.Cap.RabbitMQ.PublisherApi", "Order API")
            };

        //public static IEnumerable<Client> Clients =>
        //    new Client[] 
        //    { };


        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "3",
                    Username = "yak",
                    Password = "yakpassword"
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),//未添加导致scope错误
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "yakclient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("yaksecret".Sha256())
                    },
                    AllowedScopes = new []{ "Yak.Microservice.Product.Api", "Yak.Ocelot.Api", "Yak.Cap.RabbitMQ.SubscribeApi", "Yak.Cap.RabbitMQ.PublisherApi", IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile }//需要额外添加 
                }
            };
        }
    }
}