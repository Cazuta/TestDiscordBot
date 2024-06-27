using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using System.Linq;
using DSharpPlus.Entities;
using System.Collections.Generic;
using DSharpPlus;
using System;

namespace TestDiscordBot.commands
{
  public class TestCommands : BaseCommandModule
  {
    [Command("test")]
    public async Task MyFirstCommand(CommandContext ctx)
    {
      await ctx.Channel.SendMessageAsync($"Hello {ctx.User.Id}");
    }
    [Command("calculator")]
    public async Task СalculatorCommand(CommandContext ctx, int number1, string operation, int number2)
    {

      int result;
      char symbol = operation[0];

      switch (symbol)
      {
        case '+':
          result = number1 + number2;
          break;
        case '-':
          result = number1 - number2;
          break;
        case '/':
          if (number2 == 0)
          {
            await ctx.Channel.SendMessageAsync("Деление на ноль невозможно.");
            return;
          }
          result = number1 / number2;
          break;
        default:
          result = number1 * number2;
          break;
      }

      await ctx.Channel.SendMessageAsync($"Ваш результат: {result}");
    }
   
    [Command("sendinvite")]
    public async Task SendInvite(CommandContext ctx, ulong _id)
    {
      var user = await ctx.Guild.GetMemberAsync(_id);

      if (user == null)
      {
        await ctx.RespondAsync($"Пользователя с ID {_id} не найдено на сервере.");
        return;
      }
      var url = "https://discord.com/channels/1147523400290541578/1147523400290541581";
      await user.SendMessageAsync($"Приглашение на сервер {ctx.Guild.Id}: {url}");
      var defaultChannel = ctx.Guild.GetDefaultChannel() ?? throw new ArgumentNullException(nameof(ctx.Guild));
      await ctx.Channel.CreateInviteAsync(targetUserId: _id);
    }
    [Command("getroll")]
    public async Task GetRoll(CommandContext ctx, ulong _id, string channelName = null)
    {
      var user = await ctx.Guild.GetMemberAsync(_id);

      if (user == null)
      {
        await ctx.RespondAsync($"Пользователя с ID {_id} не найдено на сервере.");
        return;
      }

      bool foundChannel = channelName != null &&
          ctx.Guild.Channels.Any(c => c.Value.Name.ToLower() == channelName.ToLower());

      var targetChannel = foundChannel
          ? ctx.Guild.Channels.FirstOrDefault(c => c.Value.Name.ToLower() == channelName.ToLower()).Value
          : null;

      if (targetChannel == null)
      {
        await ctx.RespondAsync($"Канал с именем {channelName} не найден.");
        return;
      }


      var targetRole = ctx.Guild.Roles.FirstOrDefault(r =>
          r.Value.Permissions.HasPermission(Permissions.AccessChannels)
      );

      if (targetRole.Equals(default(KeyValuePair<ulong, DiscordRole>)))
      {
        await ctx.RespondAsync($"Не найдена роль с правами на доступ к каналу.");
        return;
      }

      await user.GrantRoleAsync(targetRole.Value);
    }
  }
  }

  


 