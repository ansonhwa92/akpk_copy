namespace FEP.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User", "LoginId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "LoginId", c => c.String());
        }
    }
}
