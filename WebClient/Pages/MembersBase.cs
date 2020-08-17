using Domain.Queries;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebClient.Abstractions;

namespace WebClient.Pages
{
    public class MembersBase: ComponentBase
    {
        protected List<FamilyMember> members = new List<FamilyMember>();
        protected List<MenuItem> leftMenuItem = new List<MenuItem>();
        protected FamilyMember member = new FamilyMember();

        protected bool showCreator;
        protected bool isLoaded;

        [Inject]
        public IMemberDataService MemberDataService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var result = await MemberDataService.GetAllMembers();

            if (result != null && result.Payload != null && result.Payload.Any())
            {
                foreach (var item in result.Payload)
                {
                    members.Add(new FamilyMember()
                    {
                        avtar = item.Avatar,
                        email = item.Email,
                        firstname = item.FirstName,
                        lastname = item.LastName,
                        role = item.Roles,
                        id = item.Id
                    });
                }
            }

            for (int i = 0; i < members.Count; i++)
            {
                var item = new MenuItem
                {
                    iconColor = members[i].avtar,
                    label = members[i].firstname,
                    referenceId = members[i].id
                };
                item.ClickCallback += onItemClick;

                leftMenuItem.Add(item);
            }
            showCreator = true;
            isLoaded = true;
        }

        protected void onAddItem()
        {
            member = new FamilyMember();
            showCreator = true;
            StateHasChanged();
        }

        protected async void onItemClick(object sender, object e)
        {
            Guid val = (Guid)e.GetType().GetProperty("referenceId").GetValue(e);
            showCreator = true;
            member.id = val;
            GetMemberQueryResult result = await MemberDataService.GetById(val);
            if (result != null)
            {
                member.avtar = result.Payload.Avatar;
                member.email = result.Payload.Email;
                member.firstname = result.Payload.FirstName;
                member.lastname = result.Payload.LastName;
                member.role = result.Payload.Roles;
            }
            StateHasChanged();
        }

        protected async Task onMemberAdd(FamilyMember familyMember)
        {
            var result = await MemberDataService.Create(new Domain.Commands.CreateMemberCommand()
            {
                Avatar = familyMember.avtar,
                FirstName = familyMember.firstname,
                LastName = familyMember.lastname,
                Email = familyMember.email,
                Roles = familyMember.role,
                Id = familyMember.id
            });

            if (result != null && result.Payload != null && result.Payload.Id != Guid.Empty)
            {
                members.Add(new FamilyMember()
                {
                    avtar = result.Payload.Avatar,
                    email = result.Payload.Email,
                    firstname = result.Payload.FirstName,
                    lastname = result.Payload.LastName,
                    role = result.Payload.Roles,
                    id = result.Payload.Id
                });

                var item = new MenuItem
                {
                    iconColor = result.Payload.Avatar,
                    label = result.Payload.FirstName,
                    referenceId = result.Payload.Id,
                };
                item.ClickCallback += onItemClick;

                leftMenuItem.Add(item);


                showCreator = false;
                StateHasChanged();
            }
        }

        protected async Task onMemberUpdate(FamilyMember familyMember)
        {
            var resultUpdate = await MemberDataService.Update(new Domain.Commands.UpdateMemberCommand()
            {
                Avatar = familyMember.avtar,
                FirstName = familyMember.firstname,
                LastName = familyMember.lastname,
                Email = familyMember.email,
                Roles = familyMember.role,
                Id = new Guid(familyMember.id.ToString())
            });

            if (resultUpdate != null && resultUpdate.Succeed)
            {
                var result = await MemberDataService.GetAllMembers();
                members = new List<FamilyMember>();
                leftMenuItem = new List<MenuItem>();

                if (result != null && result.Payload != null && result.Payload.Any())
                {
                    foreach (var item in result.Payload)
                    {
                        members.Add(new FamilyMember()
                        {
                            avtar = item.Avatar,
                            email = item.Email,
                            firstname = item.FirstName,
                            lastname = item.LastName,
                            role = item.Roles,
                            id = item.Id
                        });
                    }
                }

                for (int i = 0; i < members.Count; i++)
                {
                    var item = new MenuItem
                    {
                        iconColor = members[i].avtar,
                        label = members[i].firstname,
                        referenceId = members[i].id,
                    };
                    item.ClickCallback += onItemClick;

                    leftMenuItem.Add(item);
                }
                showCreator = true;
                isLoaded = true;
                StateHasChanged();
            }
        }

        protected async Task onMemberDelete(FamilyMember familyMember)
        {
            var resultDelete = await MemberDataService.Delete(new Domain.Commands.DeleteMemberCommand()
            {
                Id = new Guid(familyMember.id.ToString())
            });
            if (resultDelete.IsSuccess)
            {
                var result = await MemberDataService.GetAllMembers();
                members = new List<FamilyMember>();
                leftMenuItem = new List<MenuItem>();

                if (result != null && result.Payload != null && result.Payload.Any())
                {
                    foreach (var item in result.Payload)
                    {
                        members.Add(new FamilyMember()
                        {
                            avtar = item.Avatar,
                            email = item.Email,
                            firstname = item.FirstName,
                            lastname = item.LastName,
                            role = item.Roles,
                            id = item.Id
                        });
                    }
                }

                for (int i = 0; i < members.Count; i++)
                {
                    var item = new MenuItem
                    {
                        iconColor = members[i].avtar,
                        label = members[i].firstname,
                        referenceId = members[i].id,
                    };
                    item.ClickCallback += onItemClick;

                    leftMenuItem.Add(item);
                }
                showCreator = false;
                isLoaded = true;
                member = new FamilyMember();
                StateHasChanged();
            }
        }

    }
}
