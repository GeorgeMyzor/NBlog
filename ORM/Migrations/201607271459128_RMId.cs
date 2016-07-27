namespace ORM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RMId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Roles", "UserId");
            DropColumn("dbo.Users", "RoleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "RoleId", c => c.Int(nullable: false));
            AddColumn("dbo.Roles", "UserId", c => c.Int(nullable: false));
        }
    }
}
