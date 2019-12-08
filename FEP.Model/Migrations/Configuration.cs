namespace FEP.Model.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DbEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DbEntities context)
        {
            mhafeez.Seed(context);
            aiman.Seed(context);
            //   firus.Seed(context);

            //Seed Elearning Default data and Test users and sample data

            SeedElearning.Seed(context);
            SeedElearningEmail.SeedTemplateParameter(context);
            SeedElearningEmail.Seed(context);
            tajulSeed.DefaultTemplate(context);
        }
    }
}